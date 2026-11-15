using Cisco.Api.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using static Cisco.Api.Security.HandlerSupport;

namespace Cisco.Api.Security;

internal abstract partial class CustomHttpClientHandler
{
	internal async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
	{
		ValidateClientCredentials();

		_logger.LogDebug("Authenticating...");

		var attemptCount = 0;

		while (true)
		{
			using var httpClient = GetHttpClient();
			// The token request carries the same User-Agent as the API requests it authorises.
			if (Options.UserAgent is not null)
			{
				httpClient.DefaultRequestHeaders.Add("User-Agent", Options.UserAgent);
			}

			HttpResponseMessage? httpResponseMessage = null;
			try
			{
				httpResponseMessage = await httpClient
					.PostAsync(string.Empty, GetAuthBody(), cancellationToken)
					.ConfigureAwait(false);

				_logger.LogTrace("{HttpResponseMessage}", httpResponseMessage);
			}
			catch (Exception ex) when (IsTransientException(ex))
			{
				attemptCount++;
				if (await HandleTransientAuthExceptionAsync(ex, attemptCount, cancellationToken).ConfigureAwait(false))
				{
					continue;
				}
			}

			var accessTokenResponse = await DeserializeTokenResponseAsync(httpResponseMessage!, cancellationToken).ConfigureAwait(false);

			if (accessTokenResponse.Error is not null)
			{
				var attemptCountRef = new[] { attemptCount };
				var shouldContinue = await HandleAuthErrorAsync(accessTokenResponse, httpResponseMessage!, attemptCountRef, cancellationToken)
					.ConfigureAwait(false);
				attemptCount = attemptCountRef[0];
				if (shouldContinue)
				{
					continue;
				}
			}

			return StoreAndReturnAccessToken(accessTokenResponse);
		}
	}

	private async Task<bool> HandleTransientAuthExceptionAsync(Exception ex, int attemptCount, CancellationToken cancellationToken)
	{
		if (await TryRetryAsync(attemptCount, cancellationToken).ConfigureAwait(false))
		{
			return true;
		}

		_logger.LogFinalFailure(ex, $"GetAccessTokenAsync(): {ex.Message}", attemptCount);

		throw new CiscoApiException("Timeout or transient network failure during authentication.", ex);
	}

	private async Task<bool> TryRetryAsync(int attemptCount, CancellationToken cancellationToken)
	{
		if (attemptCount < Options.MaxAttemptCount)
		{
			_logger.LogWarning("Attempt {AttemptCount}/{MaxAttemptCount} failed, retrying...", attemptCount, Options.MaxAttemptCount);
			await Task.Delay(Options.RetryDelay, cancellationToken).ConfigureAwait(false);
			return true;
		}

		return false;
	}

	private void ValidateClientCredentials()
	{
		if (Options.ClientId is null)
		{
			throw new SecurityException("Options ClientId must be set");
		}

		if (Options.ClientSecret is null)
		{
			throw new SecurityException("Options ClientSecret must be set");
		}
	}

	private async Task<bool> HandleAuthErrorAsync(
		AccessTokenResponse accessTokenResponse,
		HttpResponseMessage httpResponseMessage,
		int[] attemptCount,
		CancellationToken cancellationToken)
	{
		var error = accessTokenResponse.Error!;
		var description = accessTokenResponse.ErrorDescription;
		var combinedMessage = BuildErrorMessage(error, description);

		_logger.LogDebug("Authentication failed. Error={Error} Description={Description}", error, description);

		if (!_logger.IsEnabled(LevelToLogAt) && Options.OnErrorEnsureRequestResponseHeadersLogged)
		{
			await LogResponseHeaders(httpResponseMessage, true).ConfigureAwait(false);
		}

		var isInvalidClient = error.Equals("invalid_client", StringComparison.OrdinalIgnoreCase);

		if (isInvalidClient)
		{
			if (Options.RetryInvalidClientTokenErrors)
			{
				return await HandleRetryInvalidClientErrorAsync(attemptCount, cancellationToken);
			}

			return await HandleStandardInvalidClientRetryAsync(attemptCount, cancellationToken);
		}

		throw new SecurityException(combinedMessage);
	}

	private async Task<bool> HandleRetryInvalidClientErrorAsync(int[] attemptCount, CancellationToken cancellationToken)
	{
		++attemptCount[0];
		if (attemptCount[0] < Options.RetryInvalidClientTokenErrorsMaxAttemptCount)
		{
			_logger.LogWarning("Attempt {AttemptCount}/{MaxAttemptCount} failed, retrying...",
				attemptCount[0],
				Options.RetryInvalidClientTokenErrorsMaxAttemptCount);

			await Task.Delay(Options.RetryInvalidClientTokenErrorsRetryDelay, cancellationToken).ConfigureAwait(false);
			return true;
		}

		_logger.LogFinalFailure(null, "GetAccessTokenAsync(): invalid_client", attemptCount[0]);
		throw new CiscoApiException("Timeout during authentication - gave up trying to get token after repeated invalid_client errors.");
	}

	private async Task<bool> HandleStandardInvalidClientRetryAsync(int[] attemptCount, CancellationToken cancellationToken)
	{
		++attemptCount[0];
		if (await TryRetryAsync(attemptCount[0], cancellationToken).ConfigureAwait(false))
		{
			return true;
		}

		return false;
	}

	private string StoreAndReturnAccessToken(AccessTokenResponse accessTokenResponse)
	{
		_logger.LogDebug("Authentication succeeded.");

		var expireInSeconds = accessTokenResponse.ExpiresInSeconds ?? 3540;

		if (accessTokenResponse.ExpiresInSeconds - 60 > 0)
		{
			expireInSeconds -= 60;
		}

		_logger.LogDebug("Access token should expire in {ExpireInSeconds} seconds.", expireInSeconds);

		_accessTokenExpiryDateTimeOffset = DateTimeOffset.UtcNow.AddSeconds(expireInSeconds);

		_logger.LogDebug(
			"The access token '{AccessToken}' expiry date time is '{ExpiryDateTimeUtc}'",
			accessTokenResponse.AccessToken!,
			_accessTokenExpiryDateTimeOffset
		);

		return accessTokenResponse.AccessToken!;
	}
}
