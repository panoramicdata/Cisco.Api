using Cisco.Api.Exceptions;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Headers;

using static Cisco.Api.Security.HandlerSupport;

namespace Cisco.Api.Security;

internal abstract partial class CustomFastUmbrellaHttpClientHandler
{
	private async Task<(HttpResponseMessage? Response, int AttemptCount)> SendAttemptAsync(
		int credentialIndex,
		HttpRequestMessage attemptRequest,
		HttpRequestMessage originalRequest,
		int attemptCount,
		CancellationToken cancellationToken)
	{
		try
		{
			return (await base.SendAsync(attemptRequest, cancellationToken).ConfigureAwait(false), attemptCount);
		}
		catch (Exception ex)
		{
			attemptCount++;
			if (attemptCount < Options.MaxAttemptCount)
			{
				_logger.LogWarning("Attempt {AttemptCount}/{MaxAttemptCount} failed, retrying...", attemptCount, Options.MaxAttemptCount);
				await Task.Delay(Options.RetryDelay, CancellationToken.None).ConfigureAwait(false);
				return (null, attemptCount);
			}

			if (!_logger.IsEnabled(LevelToLogAt) && Options.OnErrorEnsureRequestResponseHeadersLogged)
			{
				await LogRequestHeaders(credentialIndex, originalRequest, true).ConfigureAwait(false);
			}

			_logger.LogFinalFailure(ex, ex.Message, attemptCount);
			throw new CiscoApiException(ex.Message, ex);
		}
	}

	private async Task<bool> HandleRetriableStatusCodeAsync(
		int currentCredentialCurrentIndex,
		CiscoUmbrellaCredentialsTokenTracking currentCredentialsInstance,
		HttpResponseMessage httpResponseMessage,
		string message,
		HttpRequestMessage request,
		int[] attemptCount,
		CancellationToken cancellationToken)
	{
		if (httpResponseMessage.StatusCode == HttpStatusCode.TooManyRequests)
		{
			return await RetryAfterRateLimitAsync(httpResponseMessage, attemptCount).ConfigureAwait(false);
		}

		if (!IsTransientStatusCode(httpResponseMessage.StatusCode))
		{
			return false;
		}

		return await RetryAfterTransientFailureAsync(
			currentCredentialCurrentIndex,
			currentCredentialsInstance,
			message,
			request,
			attemptCount,
			cancellationToken).ConfigureAwait(false);
	}

	private async Task<bool> RetryAfterRateLimitAsync(HttpResponseMessage response, int[] attemptCount)
	{
		if (++attemptCount[0] >= Options.MaxAttemptCount)
		{
			return false;
		}

		var retryDelay = response.Headers.RetryAfter?.Delta ?? Options.RetryDelay;
		_logger.LogWarning(
			"Attempt {AttemptCount}/{MaxAttemptCount} failed due to a 429, retrying in {RetryDelay}...",
			attemptCount[0], Options.MaxAttemptCount, retryDelay);
		await Task.Delay(retryDelay, CancellationToken.None).ConfigureAwait(false);
		return true;
	}

	private async Task<bool> RetryAfterTransientFailureAsync(
		int credentialIndex,
		CiscoUmbrellaCredentialsTokenTracking credentials,
		string message,
		HttpRequestMessage request,
		int[] attemptCount,
		CancellationToken cancellationToken)
	{
		if (++attemptCount[0] >= Options.MaxAttemptCount)
		{
			return false;
		}

		if (message.Contains("Developer Inactive", StringComparison.Ordinal))
		{
			_logger.LogDebug("SendAsync(): Response content was Developer Inactive - could be a bad API response, requesting a new token.");
			await RefreshAccessTokenAsync(credentialIndex, credentials, cancellationToken).ConfigureAwait(false);
			request.Headers.Authorization = credentials.AuthenticationHeaderValue;
		}

		_logger.LogWarning("Attempt {AttemptCount}/{MaxAttemptCount} failed, retrying...", attemptCount[0], Options.MaxAttemptCount);
		await Task.Delay(Options.RetryDelay, CancellationToken.None).ConfigureAwait(false);
		return true;
	}
}
