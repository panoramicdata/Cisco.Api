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
			Assert.NotNull(coverageStatus.CoverageEndDate);
			Assert.NotNull(coverageStatus.IsCovered);
			Assert.NotNull(coverageStatus.SerialNumber);
		}

		[Fact]
		public async void GetCoverageSummaryBySerialNumber()
		{
			var coverageSummary = await CiscoClient.GetCoverageSummaryBySerialNumber("FTX1910100B");
			Assert.NotNull(coverageSummary);
			Assert.NotNull(coverageSummary.BasePidList);
			Assert.NotNull(coverageSummary.ContractSiteAddress1);
			Assert.NotNull(coverageSummary.ContractSiteCity);
			Assert.NotNull(coverageSummary.ContractSiteCountry);
			Assert.NotNull(coverageSummary.ContractSiteCustomerName);
			Assert.NotNull(coverageSummary.ContractSiteStateProvince);
			Assert.NotNull(coverageSummary.CoveredProductLineEndDate);
			Assert.NotNull(coverageSummary.Id);
			Assert.NotNull(coverageSummary.IsCovered);
			Assert.NotNull(coverageSummary.ParentSerialNumber);
			Assert.NotNull(coverageSummary.SerialNumber);
			Assert.NotNull(coverageSummary.ServiceContractNumber);
			Assert.NotNull(coverageSummary.ServiceLineDescription);
			Assert.NotNull(coverageSummary.WarrantyEndDate);
			Assert.NotNull(coverageSummary.WarrantyType);
			Assert.NotNull(coverageSummary.WarrantyTypeDescription);
		}

		[Fact]
		public async void GetOrderableProductIdentifierBySerialNumber()
		{
			var serialNumberOrderablePids = await CiscoClient.GetOrderableProductIdentifiersBySerialNumber("FTX1910100B");
			Assert.NotNull(serialNumberOrderablePids);
			Assert.All(serialNumberOrderablePids, CheckSerialNumberOrderablePid);
		}

		private void CheckSerialNumberOrderablePid(SerialNumberOrderablePid serialNumberOrderablePid)
		{
			Assert.NotNull(serialNumberOrderablePid.OrderablePid);
			Assert.NotNull(serialNumberOrderablePid.PillarCode);
			Assert.NotNull(serialNumberOrderablePid.PillarDescription);
		}
	}
}