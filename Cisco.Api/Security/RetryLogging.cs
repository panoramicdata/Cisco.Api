using Microsoft.Extensions.Logging;
using System;

namespace Cisco.Api.Security;

/// <summary>
/// Logs the final failure of a request or token request that has given up.
/// </summary>
/// <remarks>
/// Callers pass the number of attempts actually made, not the configured maximum: a response that is
/// never retried, such as a 403, is one attempt whatever MaxAttemptCount says. The count is only worth
/// mentioning when there was a retry, so a single attempt logs the message on its own rather than
/// "after 1 attempts".
/// </remarks>
internal static class RetryLogging
{
	internal static void LogFinalFailure(this ILogger logger, Exception? exception, string message, int attemptCount)
	{
		if (attemptCount > 1)
		{
			logger.LogError(exception, "{Message} after {AttemptCount} attempts.", message, attemptCount);
			return;
		}

		logger.LogError(exception, "{Message}", message);
	}
}
