using Cisco.Api.Data.EnterpriseAgreement.Responses;
using Cisco.Api.Data.SerialNumberToInfo;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Round-trip tests for two data models that are otherwise only exercised by integration tests
/// that require live credentials.
/// </summary>
public class DataModelRoundTripTests
{
	/// <summary>Every CommerceSku property is set and read back.</summary>
	[Fact]
	public void CommerceSku_EveryPropertyRoundTrips()
	{
		var sku = new CommerceSku();
		sku.Eol = true;
		sku.CustSuiteId = 42;
		sku.SuiteCommerceSku = "sku";
		sku.SuiteCommerceSkuDescription = "desc";
		sku.SuiteName = "name";
		sku.CustSuiteName = "cust";
		sku.EolMessage = "msg";
		sku.PurchasedEntitlements = 1;
		sku.PremierEntitlements = 2;
		sku.GrowthAllowance = 3;
		sku.TotalEntitlements = 4;
		sku.PreEAConsumption = 5;
		sku.LicenseGenerated = 6;
		sku.LicenseMigrated = 7;
		sku.C1ToDNAMigratedCount = 8;
		sku.TotalConsumption = 9;
		sku.RemainingEntitlements = 10;
		sku.SoftwareDownloads = 11;
		sku.HealthMessage = "ok";
		sku.CalculationMethod = "m";
		sku.CommitmentType = "c";

		sku.Eol.Should().BeTrue();
		sku.CustSuiteId.Should().Be(42);
		sku.SuiteCommerceSku.Should().Be("sku");
		sku.SuiteCommerceSkuDescription.Should().Be("desc");
		sku.SuiteName.Should().Be("name");
		sku.CustSuiteName.Should().Be("cust");
		sku.EolMessage.Should().Be("msg");
		sku.PurchasedEntitlements.Should().Be(1);
		sku.PremierEntitlements.Should().Be(2);
		sku.GrowthAllowance.Should().Be(3);
		sku.TotalEntitlements.Should().Be(4);
		sku.PreEAConsumption.Should().Be(5);
		sku.LicenseGenerated.Should().Be(6);
		sku.LicenseMigrated.Should().Be(7);
		sku.C1ToDNAMigratedCount.Should().Be(8);
		sku.TotalConsumption.Should().Be(9);
		sku.RemainingEntitlements.Should().Be(10);
		sku.SoftwareDownloads.Should().Be(11);
		sku.HealthMessage.Should().Be("ok");
		sku.CalculationMethod.Should().Be("m");
		sku.CommitmentType.Should().Be("c");
	}

	/// <summary>The static Create method populates every property.</summary>
	[Fact]
	public void CommerceSku_Create_SetsEveryProperty()
	{
		var sku = CommerceSku.Create(
			eol: true, custSuiteId: 1, suiteCommerceSku: "s1", suiteCommerceSkuDescription: "d1",
			suiteName: "n1", custSuiteName: "c1", eolMessage: "m1", purchasedEntitlements: 2,
			premierEntitlements: 3, growthAllowance: 4, totalEntitlements: 5, preEAConsumption: 6,
			licenseGenerated: 7, licenseMigrated: 8, c1ToDNAMigratedCount: 9, totalConsumption: 10,
			remainingEntitlements: 11, softwareDownloads: 12, healthMessage: "h1", calculationMethod: "cm",
			commitmentType: "ct");

		sku.Eol.Should().BeTrue();
		sku.CustSuiteId.Should().Be(1);
		sku.SuiteCommerceSku.Should().Be("s1");
		sku.SuiteCommerceSkuDescription.Should().Be("d1");
		sku.SuiteName.Should().Be("n1");
		sku.CustSuiteName.Should().Be("c1");
		sku.EolMessage.Should().Be("m1");
		sku.PurchasedEntitlements.Should().Be(2);
		sku.PremierEntitlements.Should().Be(3);
		sku.GrowthAllowance.Should().Be(4);
		sku.TotalEntitlements.Should().Be(5);
		sku.PreEAConsumption.Should().Be(6);
		sku.LicenseGenerated.Should().Be(7);
		sku.LicenseMigrated.Should().Be(8);
		sku.C1ToDNAMigratedCount.Should().Be(9);
		sku.TotalConsumption.Should().Be(10);
		sku.RemainingEntitlements.Should().Be(11);
		sku.SoftwareDownloads.Should().Be(12);
		sku.HealthMessage.Should().Be("h1");
		sku.CalculationMethod.Should().Be("cm");
		sku.CommitmentType.Should().Be("ct");
	}

	/// <summary>Every CoverageSummary property is set and read back.</summary>
	[Fact]
	public void CoverageSummary_EveryPropertyRoundTrips()
	{
		var summary = new CoverageSummary();
		summary.BasePid = "b";
		summary.ContractSiteAddress1 = "a1";
		summary.ContractSiteCity = "city";
		summary.ContractSiteCountry = "us";
		summary.ContractSiteCustomerName = "cust";
		summary.ContractSiteStateProvince = "ca";
		summary.CoveredProductLineEndDate = "2025-01-01";
		summary.Id = "i";
		summary.IsCovered = "YES";
		summary.ItemDescription = "desc";
		summary.ItemPosition = "P";
		summary.ItemType = "chassis";
		summary.LastIndex = 1;
		summary.OrderablePid = "opid";
		summary.PageIndex = 2;
		summary.PageRecordCount = "10";
		summary.ParentSerialNumber = "psn";
		summary.PillarCode = "3";
		summary.SelfLink = "http://x";
		summary.ServiceContractNumber = "scn";
		summary.ServiceLineDescription = "sl";
		summary.SerialNumber = "sn";
		summary.Title = "t";
		summary.TotalRecordCount = "100";
		summary.WarrantyEndDate = "2026-01-01";
		summary.WarrantyType = "w";
		summary.WarrantyTypeDescription = "wd";

		summary.BasePid.Should().Be("b");
		summary.ContractSiteAddress1.Should().Be("a1");
		summary.ContractSiteCity.Should().Be("city");
		summary.ContractSiteCountry.Should().Be("us");
		summary.ContractSiteCustomerName.Should().Be("cust");
		summary.ContractSiteStateProvince.Should().Be("ca");
		summary.CoveredProductLineEndDate.Should().Be("2025-01-01");
		summary.Id.Should().Be("i");
		summary.IsCovered.Should().Be("YES");
		summary.ItemDescription.Should().Be("desc");
		summary.ItemPosition.Should().Be("P");
		summary.ItemType.Should().Be("chassis");
		summary.LastIndex.Should().Be(1);
		summary.OrderablePid.Should().Be("opid");
		summary.PageIndex.Should().Be(2);
		summary.PageRecordCount.Should().Be("10");
		summary.ParentSerialNumber.Should().Be("psn");
		summary.PillarCode.Should().Be("3");
		summary.SelfLink.Should().Be("http://x");
		summary.ServiceContractNumber.Should().Be("scn");
		summary.ServiceLineDescription.Should().Be("sl");
		summary.SerialNumber.Should().Be("sn");
		summary.Title.Should().Be("t");
		summary.TotalRecordCount.Should().Be("100");
		summary.WarrantyEndDate.Should().Be("2026-01-01");
		summary.WarrantyType.Should().Be("w");
		summary.WarrantyTypeDescription.Should().Be("wd");
	}
}
