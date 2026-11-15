using Cisco.Api.Exceptions;
using Microsoft.Extensions.Logging;
using System.Security;

namespace Cisco.Api.Security;

/// <summary>
/// The token request loop shared by the single-credential and multi-credential handlers.
/// </summary>
internal static class TokenAcquisition
{
	private const LogLevel LevelToLogAt = LogLevel.Trace;

	internal static async Task<AccessTokenResponse> AcquireAsync(
		ILogger logger,
		CiscoClientOptions options,
		Func<HttpClient> getHttpClient,
		Func<StringContent> getAuthBody,
		Func<AccessTokenResponse, HttpResponseMessage, int[], Task<bool>> handleError,
		CancellationToken cancellationToken)
	{
		logger.LogDebug("Authenticating...");

		var attemptCount = 0;

		while (true)
		{
			using var httpClient = getHttpClient();
			// The token request carries the same User-Agent as the API requests it authorises.
			if (options.UserAgent is not null)
			{
				httpClient.DefaultRequestHeaders.Add("User-Agent", options.UserAgent);
			}

			HttpResponseMessage? httpResponseMessage = null;
			try
			{
				httpResponseMessage = await httpClient
					.PostAsync(string.Empty, getAuthBody(), cancellationToken)
					.ConfigureAwait(false);

				logger.LogTrace("{HttpResponseMessage}", httpResponseMessage);
			}
			catch (Exception ex) when (HandlerSupport.IsTransientException(ex))
			{
				attemptCount++;
				await RetryTransientAsync(logger, options, ex, attemptCount, cancellationToken).ConfigureAwait(false);
				continue;
			}

			var accessTokenResponse = await HandlerSupport.DeserializeTokenResponseAsync(httpResponseMessage, cancellationToken).ConfigureAwait(false);
			if (accessTokenResponse.Error is null)
			{
				return accessTokenResponse;
			}

			var attemptCountRef = new[] { attemptCount };
			var shouldContinue = await handleError(accessTokenResponse, httpResponseMessage, attemptCountRef).ConfigureAwait(false);
			attemptCount = attemptCountRef[0];
			if (!shouldContinue)
			{
				return accessTokenResponse;
			}
		}
	}

	internal static async Task<bool> HandleErrorAsync(
		ILogger logger,
		CiscoClientOptions options,
		AccessTokenResponse accessTokenResponse,
		HttpResponseMessage httpResponseMessage,
		int[] attemptCount,
		Func<HttpResponseMessage, Task> logResponseAsError,
		CancellationToken cancellationToken)
	{
		var error = accessTokenResponse.Error!;
		var description = accessTokenResponse.ErrorDescription;

		logger.LogDebug("Authentication failed. Error={Error} Description={Description}", error, description);

		if (!logger.IsEnabled(LevelToLogAt) && options.OnErrorEnsureRequestResponseHeadersLogged)
		{
			await logResponseAsError(httpResponseMessage).ConfigureAwait(false);
		}

		if (!error.Equals("invalid_client", StringComparison.OrdinalIgnoreCase))
		{
			throw new SecurityException(HandlerSupport.BuildErrorMessage(error, description));
		}

		return options.RetryInvalidClientTokenErrors
			? await RetryInvalidClientAsync(logger, options, attemptCount, cancellationToken).ConfigureAwait(false)
			: await RetryStandardInvalidClientAsync(logger, options, attemptCount, cancellationToken).ConfigureAwait(false);
	}

	internal static int ExpirySeconds(ILogger logger, AccessTokenResponse accessTokenResponse)
	{
		logger.LogDebug("Authentication succeeded.");

		var expireInSeconds = accessTokenResponse.ExpiresInSeconds ?? 3540;

		if (accessTokenResponse.ExpiresInSeconds - 60 > 0)
		{
			expireInSeconds -= 60;
		}

		logger.LogDebug("Access token should expire in {ExpireInSeconds} seconds.", expireInSeconds);

		return expireInSeconds;
	}

	private static async Task RetryTransientAsync(
		ILogger logger,
		CiscoClientOptions options,
		Exception ex,
		int attemptCount,
		CancellationToken cancellationToken)
	{
		if (attemptCount < options.MaxAttemptCount)
		{
			logger.LogWarning("GetAccessTokenAsync(): Attempt {AttemptCount}/{MaxAttemptCount} failed, retrying...",
				attemptCount,
				options.MaxAttemptCount
			);

			await Task.Delay(options.RetryDelay, cancellationToken).ConfigureAwait(false);
			return;
		}

		logger.LogFinalFailure(ex, $"GetAccessTokenAsync(): {ex.Message}", attemptCount);

		throw new CiscoApiException("Timeout or transient network failure during authentication.", ex);
	}

	private static async Task<bool> RetryInvalidClientAsync(
		ILogger logger,
		CiscoClientOptions options,
		int[] attemptCount,
		CancellationToken cancellationToken)
	{
		if (++attemptCount[0] < options.RetryInvalidClientTokenErrorsMaxAttemptCount)
		{
			logger.LogWarning("GetAccessTokenAsync(): invalid_client ({AttemptCount}/{MaxAttemptCount}) retrying after {Delay}s...",
				attemptCount[0],
				options.RetryInvalidClientTokenErrorsMaxAttemptCount,
				options.RetryInvalidClientTokenErrorsRetryDelay.TotalSeconds);

			await Task.Delay(options.RetryInvalidClientTokenErrorsRetryDelay, cancellationToken).ConfigureAwait(false);
			return true;
		}

		logger.LogFinalFailure(null, "GetAccessTokenAsync(): invalid_client", attemptCount[0]);
		throw new CiscoApiException("Timeout during authentication - gave up trying to get token after repeated invalid_client errors.");
	}

	private static async Task<bool> RetryStandardInvalidClientAsync(
		ILogger logger,
		CiscoClientOptions options,
		int[] attemptCount,
		CancellationToken cancellationToken)
	{
		if (++attemptCount[0] >= options.MaxAttemptCount)
		{
			return false;
		}

		logger.LogWarning("GetAccessTokenAsync(): invalid_client ({AttemptCount}/{MaxAttemptCount}) using standard retry settings, retrying after {Delay}s...",
			attemptCount[0],
			options.MaxAttemptCount,
			options.RetryDelay.TotalSeconds);
		await Task.Delay(options.RetryDelay, cancellationToken).ConfigureAwait(false);
		return true;
	}
}
