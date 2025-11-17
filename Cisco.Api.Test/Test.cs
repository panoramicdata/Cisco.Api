using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Xunit.Abstractions;

namespace Cisco.Api.Test;

public abstract class Test
{
	protected Test(ITestOutputHelper iTestOutputHelper)
	{
		Logger = iTestOutputHelper.BuildLoggerFor<Test>();
		Config = new TestPortalConfig(null, Logger);
		CiscoClient = Config.CiscoClient;
		Stopwatch = Stopwatch.StartNew();
	}
	protected ILogger Logger { get; }

	internal TestPortalConfig Config { get; }

	private Stopwatch Stopwatch { get; }

	protected CiscoClient CiscoClient { get; }

	protected void AssertIsFast(int durationSeconds)
		=> Stopwatch.ElapsedMilliseconds.Should().BeInRange(0, durationSeconds * 1000);
}