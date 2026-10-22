using Cisco.Api.Security;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Tests for the final-failure line the handlers log when a request or token request gives up.
///
/// <para>
/// That line used to report the configured maximum rather than the attempts actually made, so a 403,
/// which is never retried, read "Forbidden: ... after 1 attempts." - and would have claimed three
/// attempts with MaxAttemptCount set to 3.
/// </para>
///
/// <para>
/// These are pure unit tests. They require no credentials, no configuration and no live API.
/// </para>
/// </summary>
public class RetryLoggingTests
{
	/// <summary>
	/// A single attempt was never retried, so the count adds nothing and is left off.
	/// </summary>
	[Fact]
	public void LogFinalFailure_SingleAttempt_OmitsTheAttemptCount()
	{
		var logger = new CapturingLogger();

		logger.LogFinalFailure(null, "Forbidden: {\"status\":\"ERROR\"}", 1);

		logger.Messages.Should().ContainSingle()
			.Which.Should().Be("Forbidden: {\"status\":\"ERROR\"}");
	}

	/// <summary>
	/// A retried failure reports the real number of attempts, in the plural.
	/// </summary>
	[Fact]
	public void LogFinalFailure_SeveralAttempts_ReportsTheAttemptsMade()
	{
		var logger = new CapturingLogger();

		logger.LogFinalFailure(null, "ServiceUnavailable", 3);

		logger.Messages.Should().ContainSingle()
			.Which.Should().Be("ServiceUnavailable after 3 attempts.");
	}

	/// <summary>
	/// The exception still reaches the logger, so the stack trace is not lost.
	/// </summary>
	[Fact]
	public void LogFinalFailure_WithException_PassesTheExceptionAtErrorLevel()
	{
		var logger = new CapturingLogger();
		var exception = new TimeoutException("The operation has timed out.");

		logger.LogFinalFailure(exception, exception.Message, 2);

		logger.Entries.Should().ContainSingle()
			.Which.Should().Be((LogLevel.Error, exception));
	}

	private sealed class CapturingLogger : ILogger
	{
		public List<string> Messages { get; } = [];

		public List<(LogLevel Level, Exception? Exception)> Entries { get; } = [];

		public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

		public bool IsEnabled(LogLevel logLevel) => true;

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
		{
			Messages.Add(formatter(state, exception));
			Entries.Add((logLevel, exception));
		}
	}
}
