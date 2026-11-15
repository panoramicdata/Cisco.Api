using Cisco.Api.Exceptions;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Security;

using static Cisco.Api.Security.HandlerSupport;

namespace Cisco.Api.Security;

internal abstract partial class CustomFastUmbrellaHttpClientHandler
{
	internal async Task<string> GetAccessTokenAsync(int currentCredentialCurrentIndex, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Authenticating...");

		var attemptCount = 0;

		while (true)
		{
			using var httpClient = GetHttpClient(currentCredentialCurrentIndex);
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
				++attemptCount;
				if (await HandleTransientAuthExceptionAsync(ex, attemptCount, cancellationToken).ConfigureAwait(false))
				{
					continue;
				}
			}

				var accessTokenResponse = await DeserializeTokenResponseAsync(httpResponseMessage!, cancellationToken).ConfigureAwait(false);

			if (accessTokenResponse.Error is not null)
			{
				var attemptCountRef = new[] { attemptCount };
				var shouldContinue = await HandleAuthErrorAsync(currentCredentialCurrentIndex, accessTokenResponse, httpResponseMessage!, attemptCountRef, cancellationToken)
					.ConfigureAwait(false);
				attemptCount = attemptCountRef[0];
				if (shouldContinue)
				{
					continue;
				}
			}

			return StoreAndReturnAccessToken(currentCredentialCurrentIndex, accessTokenResponse);
		}
	}

	private async Task<bool> HandleTransientAuthExceptionAsync(Exception ex, int attemptCount, CancellationToken cancellationToken)
	{
		if (attemptCount < Options.MaxAttemptCount)
		{
			_logger.LogWarning("GetAccessTokenAsync(): Attempt {AttemptCount}/{MaxAttemptCount} failed, retrying...",
				attemptCount,
				Options.MaxAttemptCount
			);

			await Task.Delay(Options.RetryDelay, cancellationToken)
				.ConfigureAwait(false);

			return true;
		}

		_logger.LogFinalFailure(ex, $"GetAccessTokenAsync(): {ex.Message}", attemptCount);

		throw new CiscoApiException("Timeout or transient network failure during authentication.", ex);
	}

	private async Task<bool> HandleAuthErrorAsync(
		int currentCredentialCurrentIndex,
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
			await LogResponseHeaders(currentCredentialCurrentIndex, httpResponseMessage, true).ConfigureAwait(false);
		}

		var isInvalidClient = error.Equals("invalid_client", StringComparison.OrdinalIgnoreCase);

		if (isInvalidClient)
		{
			if (Options.RetryInvalidClientTokenErrors)
			{
				return await HandleRetryInvalidClientErrorAsync(attemptCount, cancellationToken).ConfigureAwait(false);
			}

			return await HandleStandardInvalidClientRetryAsync(attemptCount, cancellationToken).ConfigureAwait(false);
		}

		throw new SecurityException(combinedMessage);
	}

	private async Task<bool> HandleRetryInvalidClientErrorAsync(int[] attemptCount, CancellationToken cancellationToken)
	{
		if (++attemptCount[0] < Options.RetryInvalidClientTokenErrorsMaxAttemptCount)
		{
			_logger.LogWarning("GetAccessTokenAsync(): invalid_client ({AttemptCount}/{MaxAttemptCount}) – retrying after {Delay}s...",
				attemptCount[0],
				Options.RetryInvalidClientTokenErrorsMaxAttemptCount,
				Options.RetryInvalidClientTokenErrorsRetryDelay.TotalSeconds);

			await Task.Delay(Options.RetryInvalidClientTokenErrorsRetryDelay, cancellationToken).ConfigureAwait(false);
			return true;
		}

		_logger.LogFinalFailure(null, "GetAccessTokenAsync(): invalid_client", attemptCount[0]);
		throw new CiscoApiException("Timeout during authentication - gave up trying to get token after repeated invalid_client errors.");
	}

	private async Task<bool> HandleStandardInvalidClientRetryAsync(int[] attemptCount, CancellationToken cancellationToken)
	{
		if (++attemptCount[0] < Options.MaxAttemptCount)
		{
			_logger.LogWarning("GetAccessTokenAsync(): invalid_client ({AttemptCount}/{MaxAttemptCount}) using standard retry settings, retrying after {Delay}s...",
				attemptCount[0],
				Options.MaxAttemptCount,
				Options.RetryDelay.TotalSeconds);
			await Task.Delay(Options.RetryDelay, cancellationToken).ConfigureAwait(false);
			return true;
		}

		return false;
	}

	private string StoreAndReturnAccessToken(int currentCredentialCurrentIndex, AccessTokenResponse accessTokenResponse)
	{
		_logger.LogDebug("Authentication succeeded.");

		var expireInSeconds = accessTokenResponse.ExpiresInSeconds ?? 3540;

		if (accessTokenResponse.ExpiresInSeconds - 60 > 0)
		{
			expireInSeconds -= 60;
		}

		_logger.LogDebug("Access token should expire in {ExpireInSeconds} seconds.", expireInSeconds);

		CiscoUmbrellaCredentials.ElementAt(currentCredentialCurrentIndex).Value.AccessTokenExpiryDateTimeOffset = DateTimeOffset.UtcNow.AddSeconds(expireInSeconds);

		_logger.LogDebug(
			"The access token '{AccessToken}' expiry date time is '{ExpiryDateTimeUtc}'",
			accessTokenResponse.AccessToken!,
			CiscoUmbrellaCredentials.ElementAt(currentCredentialCurrentIndex).Value.AccessTokenExpiryDateTimeOffset
		);

		return accessTokenResponse.AccessToken!;
	}

	private async Task EnsureAuthenticatedAsync(int currentCredentialCurrentIndex, CiscoUmbrellaCredentialsTokenTracking currentCredentialsInstance, CancellationToken cancellationToken)
	{
		if (currentCredentialsInstance.AccessTokenExpiryDateTimeOffset is not null
			&& currentCredentialsInstance.AccessTokenExpiryDateTimeOffset <= DateTimeOffset.UtcNow)
		{
			_logger.LogDebug("SendAsync(): The access token expiry date time ('{AccessTokenExpiryDateTimeOffset}') has expired - getting a new token...", currentCredentialsInstance.AccessTokenExpiryDateTimeOffset);
		}

		if (currentCredentialsInstance.AuthenticationHeaderValue is null
			|| (currentCredentialsInstance.AccessTokenExpiryDateTimeOffset is not null
				&& currentCredentialsInstance.AccessTokenExpiryDateTimeOffset <= DateTimeOffset.UtcNow))
		{
			await RefreshAccessTokenAsync(currentCredentialCurrentIndex, currentCredentialsInstance, cancellationToken).ConfigureAwait(false);
		}
	}

	private async Task RefreshAccessTokenAsync(int currentCredentialCurrentIndex, CiscoUmbrellaCredentialsTokenTracking currentCredentialsInstance, CancellationToken cancellationToken)
	{
		currentCredentialsInstance.AccessToken = await GetAccessTokenAsync(currentCredentialCurrentIndex, cancellationToken).ConfigureAwait(false);
		currentCredentialsInstance.AuthenticationHeaderValue = new AuthenticationHeaderValue("Bearer", currentCredentialsInstance.AccessToken);
	}
}
