using Cisco.Api.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using static Cisco.Api.Security.HandlerSupport;

namespace Cisco.Api.Security;

internal abstract partial class CustomHttpClientHandler
{
	private async Task<(HttpResponseMessage? Response, int AttemptCount)> SendAttemptAsync(
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
			if (await TryRetryAsync(attemptCount, CancellationToken.None).ConfigureAwait(false))
			{
				return (null, attemptCount);
			}

			if (!_logger.IsEnabled(LevelToLogAt) && Options.OnErrorEnsureRequestResponseHeadersLogged)
			{
				await LogRequestHeaders(originalRequest, true).ConfigureAwait(false);
			}

			_logger.LogFinalFailure(ex, ex.Message, attemptCount);
			throw new CiscoApiException(ex.Message, ex);
		}
	}

	private async Task<bool> HandleRetriableStatusCodeAsync(
		HttpResponseMessage httpResponseMessage,
		string message,
		HttpRequestMessage request,
		int[] attemptCount,
		CancellationToken cancellationToken)
	{
		if (httpResponseMessage.StatusCode == HttpStatusCode.TooManyRequests)
		{
			return await RetryAfterRateLimitAsync(attemptCount).ConfigureAwait(false);
		}

		if (!IsTransientStatusCode(httpResponseMessage.StatusCode))
		{
			return false;
		}

		return await RetryAfterTransientFailureAsync(message, request, attemptCount, cancellationToken).ConfigureAwait(false);
	}

	private async Task<bool> RetryAfterRateLimitAsync(int[] attemptCount)
	{
		++attemptCount[0];
		return await TryRetryAsync(attemptCount[0], CancellationToken.None).ConfigureAwait(false);
	}

	private async Task<bool> RetryAfterTransientFailureAsync(
		string message,
		HttpRequestMessage request,
		int[] attemptCount,
		CancellationToken cancellationToken)
	{
		++attemptCount[0];
		if (!await TryRetryAsync(attemptCount[0], CancellationToken.None).ConfigureAwait(false))
		{
			return false;
		}

		if (message.Contains("Developer Inactive", StringComparison.Ordinal))
		{
			_logger.LogDebug("SendAsync(): Response content was Developer Inactive - could be a bad API response, requesting a new token.");
			var refreshedToken = await GetAccessTokenAsync(cancellationToken).ConfigureAwait(false);
			_authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", refreshedToken);
			request.Headers.Authorization = _authenticationHeaderValue;
		}

		return true;
	}
}
