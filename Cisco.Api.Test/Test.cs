using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Provides shared test infrastructure.
/// </summary>
public abstract class Test
{
	/// <summary>
	/// Initializes a new instance of the <see cref="Test"/> class.
	/// </summary>
	protected Test(ITestOutputHelper iTestOutputHelper)
	{
		Logger = iTestOutputHelper.BuildLoggerFor<Test>();
		Config = new TestPortalConfig(null, Logger);
		CiscoClient = Config.CiscoClient;
		Stopwatch = Stopwatch.StartNew();
	}
	/// <summary>
	/// Gets or sets the logger.
	/// </summary>
	protected ILogger Logger { get; }

	internal TestPortalConfig Config { get; }

	private Stopwatch Stopwatch { get; }

	/// <summary>
	/// Gets or sets the Cisco client.
	/// </summary>
	protected CiscoClient CiscoClient { get; }

	/// <summary>
	/// Performs the assert is fast operation.
	/// </summary>
	protected void AssertIsFast(int durationSeconds)
		=> Stopwatch.ElapsedMilliseconds.Should().BeInRange(0, durationSeconds * 1000);
}