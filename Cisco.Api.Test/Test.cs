using System.Diagnostics;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test
{
	public abstract class Test
	{
		protected Test(ITestOutputHelper iTestOutputHelper)
		{
			Output = iTestOutputHelper;
			CiscoClient = new TestPortalConfig(null, iTestOutputHelper).CiscoClient;
			Stopwatch = Stopwatch.StartNew();
		}

		private Stopwatch Stopwatch { get; }

		protected ITestOutputHelper Output { get; }

		protected CiscoClient CiscoClient { get; }

		protected void AssertIsFast(int durationSeconds)
			=> Assert.InRange(Stopwatch.ElapsedMilliseconds, 0, durationSeconds * 1000);

		protected CiscoClient GetCiscoClient(string customerName) => new TestPortalConfig(customerName, Output).CiscoClient;
	}
}