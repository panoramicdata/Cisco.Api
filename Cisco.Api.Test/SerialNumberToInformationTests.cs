using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test
{
	public class SerialNumberToInformationTests : Test
	{
		public SerialNumberToInformationTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetCoverageStatusBySerialNumber()
		{
			var coverageStatus = await CiscoClient.GetCoverageStatusBySerialNumber("FTX1910100B");
			Assert.NotNull(coverageStatus);
		}

		[Fact]
		public async void GetCoverageSummaryBySerialNumber()
		{
			var coverageSummary = await CiscoClient.GetCoverageSummaryBySerialNumber("FTX1910100B");
			Assert.NotNull(coverageSummary);
		}

		[Fact]
		public async void GetOrderableProductIdentifierBySerialNumber()
		{
			var serialNumberOrderablePids = await CiscoClient.GetOrderableProductIdentifiersBySerialNumber("FTX1910100B");
			Assert.NotNull(serialNumberOrderablePids);
		}
	}
}