using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test;

public class SerialNumberToInfoTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
	[Fact]
	public async Task GetCoverageStatusBySerialNumber()
	{
		var coverageStatusCollection = await CiscoClient
			.SerialNumberToInfo
			.GetCoverageStatusBySerialNumbersAsync(new[] { "FTX1910100B" })
			.ConfigureAwait(true);

		coverageStatusCollection.Should().NotBeNull();
		coverageStatusCollection.CoverageStatuses.Select(c => c.CoverageEndDate).Should().NotBeNullOrEmpty();
		coverageStatusCollection.CoverageStatuses.Select(c => c.IsCovered).Should().NotBeNullOrEmpty();
		coverageStatusCollection.CoverageStatuses.Select(c => c.SerialNumber).Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetCoverageSummaryBySerialNumber()
	{
		var coverageSummaryCollection = await CiscoClient
			.SerialNumberToInfo
			.GetCoverageSummaryBySerialNumbersAsync(new[] { "FTX1910100B" })
			.ConfigureAwait(true);
		coverageSummaryCollection.Should().NotBeNull();
		coverageSummaryCollection.CoverageSummaries.Select(cs => cs.BasePid).Should().NotBeNullOrEmpty();
		coverageSummaryCollection.CoverageSummaries.Select(cs => cs.ContractSiteAddress1).Should().NotBeNullOrEmpty();
		coverageSummaryCollection.CoverageSummaries.Select(cs => cs.ContractSiteCity).Should().NotBeNullOrEmpty();
		coverageSummaryCollection.CoverageSummaries.Select(cs => cs.ContractSiteCountry).Should().NotBeNullOrEmpty();
		coverageSummaryCollection.CoverageSummaries.Select(cs => cs.ContractSiteCustomerName).Should().NotBeNullOrEmpty();
		coverageSummaryCollection.CoverageSummaries.Select(cs => cs.ContractSiteStateProvince).Should().NotBeNullOrEmpty();
		coverageSummaryCollection.CoverageSummaries.Select(cs => cs.CoveredProductLineEndDate).Should().NotBeNullOrEmpty();
		coverageSummaryCollection.CoverageSummaries.Select(cs => cs.Id).Should().NotBeNullOrEmpty();
		coverageSummaryCollection.CoverageSummaries.Select(cs => cs.IsCovered).Should().NotBeNullOrEmpty();
		coverageSummaryCollection.CoverageSummaries.Select(cs => cs.ParentSerialNumber).Should().NotBeNullOrEmpty();
		coverageSummaryCollection.CoverageSummaries.Select(cs => cs.SerialNumber).Should().NotBeNullOrEmpty();
		coverageSummaryCollection.CoverageSummaries.Select(cs => cs.ServiceContractNumber).Should().NotBeNullOrEmpty();
		coverageSummaryCollection.CoverageSummaries.Select(cs => cs.ServiceLineDescription).Should().NotBeNullOrEmpty();
		coverageSummaryCollection.CoverageSummaries.Select(cs => cs.WarrantyEndDate).Should().NotBeNullOrEmpty();
		coverageSummaryCollection.CoverageSummaries.Select(cs => cs.WarrantyType).Should().NotBeNullOrEmpty();
		coverageSummaryCollection.CoverageSummaries.Select(cs => cs.WarrantyTypeDescription).Should().NotBeNullOrEmpty();
	}

	[Fact]
	public async Task GetOrderableProductIdentifierBySerialNumber()
	{
		var serialNumberOrderablePids = await CiscoClient
			.SerialNumberToInfo
			.GetOrderableProductIdentifiersBySerialNumbersAsync(new[] { "FTX1910100B" })
			.ConfigureAwait(true);
		serialNumberOrderablePids.Should().NotBeNull();
		serialNumberOrderablePids.SerialNumberOrderablePids.Select(p => p.SerialNumber).Should().NotBeNullOrEmpty();
		serialNumberOrderablePids.SerialNumberOrderablePids.Select(p => p.IsCovered).Should().NotBeNullOrEmpty();
		serialNumberOrderablePids.SerialNumberOrderablePids.Select(p => p.CoverageEndDate).Should().NotBeNullOrEmpty();
	}
}