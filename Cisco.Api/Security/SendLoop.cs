using Cisco.Api.Exceptions;

namespace Cisco.Api.Security;

/// <summary>
/// The per-handler steps the shared send loop calls back into.
/// </summary>
internal sealed record SendSteps(
	Func<HttpRequestMessage, HttpRequestMessage, int, Task<(HttpResponseMessage? Response, int AttemptCount)>> Attempt,
	Func<HttpResponseMessage, Task> LogResponse,
	Func<HttpResponseMessage, string, HttpRequestMessage, int[], Task<bool>> ShouldRetry,
	Func<HttpRequestMessage, HttpResponseMessage, string, int, Task<CiscoApiException>> CreateException);

/// <summary>
/// The retry loop shared by the single-credential and multi-credential handlers.
/// </summary>
internal static class SendLoop
{
	internal static async Task<HttpResponseMessage> RunAsync(HttpRequestMessage request, SendSteps steps, CancellationToken cancellationToken)
	{
		var requestBodyBytes = await HandlerSupport.ReadRequestBodyAsync(request, cancellationToken).ConfigureAwait(false);
		var attemptCount = 0;
		while (true)
		{
			using var attemptRequest = HandlerSupport.CloneRequest(request, requestBodyBytes);
			var attempt = await steps.Attempt(attemptRequest, request, attemptCount).ConfigureAwait(false);
			attemptCount = attempt.AttemptCount;
			if (attempt.Response is null)
			{
				continue;
			}

			await steps.LogResponse(attempt.Response).ConfigureAwait(false);
			if (attempt.Response.IsSuccessStatusCode)
			{
				return attempt.Response;
			}

			var message = await HandlerSupport.GetResponseContent(attempt.Response.StatusCode, attempt.Response.Content).ConfigureAwait(false);
			var attemptCountRef = new[] { attemptCount };
			if (await steps.ShouldRetry(attempt.Response, message, request, attemptCountRef).ConfigureAwait(false))
			{
				attemptCount = attemptCountRef[0];
				continue;
			}

			// attemptCount counts the failures before this response, so this response was attempt attemptCount + 1.
			throw await steps.CreateException(request, attempt.Response, message, attemptCount + 1).ConfigureAwait(false);
		}
	}
}