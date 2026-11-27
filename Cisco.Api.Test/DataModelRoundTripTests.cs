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
		=> PropertyAssert.SetAndRead<CommerceSku>(
			(nameof(CommerceSku.Eol), true),
			(nameof(CommerceSku.CustSuiteId), 42),
			(nameof(CommerceSku.SuiteCommerceSku), "sku"),
			(nameof(CommerceSku.SuiteCommerceSkuDescription), "desc"),
			(nameof(CommerceSku.SuiteName), "name"),
			(nameof(CommerceSku.CustSuiteName), "cust"),
			(nameof(CommerceSku.EolMessage), "msg"),
			(nameof(CommerceSku.PurchasedEntitlements), 1),
			(nameof(CommerceSku.PremierEntitlements), 2),
			(nameof(CommerceSku.GrowthAllowance), 3),
			(nameof(CommerceSku.TotalEntitlements), 4),
			(nameof(CommerceSku.PreEAConsumption), 5),
			(nameof(CommerceSku.LicenseGenerated), 6),
			(nameof(CommerceSku.LicenseMigrated), 7),
			(nameof(CommerceSku.C1ToDNAMigratedCount), 8),
			(nameof(CommerceSku.TotalConsumption), 9),
			(nameof(CommerceSku.RemainingEntitlements), 10),
			(nameof(CommerceSku.SoftwareDownloads), 11),
			(nameof(CommerceSku.HealthMessage), "ok"),
			(nameof(CommerceSku.CalculationMethod), "m"),
			(nameof(CommerceSku.CommitmentType), "c"));

	/// <summary>An object initializer populates every property.</summary>
	[Fact]
	public void CommerceSku_Initializer_SetsEveryProperty()
	{
		var sku = new CommerceSku
		{
			Eol = true,
			CustSuiteId = 1,
			SuiteCommerceSku = "s1",
			SuiteCommerceSkuDescription = "d1",
			SuiteName = "n1",
			CustSuiteName = "c1",
			EolMessage = "m1",
			PurchasedEntitlements = 2,
			PremierEntitlements = 3,
			GrowthAllowance = 4,
			TotalEntitlements = 5,
			PreEAConsumption = 6,
			LicenseGenerated = 7,
			LicenseMigrated = 8,
			C1ToDNAMigratedCount = 9,
			TotalConsumption = 10,
			RemainingEntitlements = 11,
			SoftwareDownloads = 12,
			HealthMessage = "h1",
			CalculationMethod = "cm",
			CommitmentType = "ct",
		};

		PropertyAssert.AssertValues(
			sku,
			(nameof(CommerceSku.Eol), true),
			(nameof(CommerceSku.CustSuiteId), 1),
			(nameof(CommerceSku.SuiteCommerceSku), "s1"),
			(nameof(CommerceSku.SuiteCommerceSkuDescription), "d1"),
			(nameof(CommerceSku.SuiteName), "n1"),
			(nameof(CommerceSku.CustSuiteName), "c1"),
			(nameof(CommerceSku.EolMessage), "m1"),
			(nameof(CommerceSku.PurchasedEntitlements), 2),
			(nameof(CommerceSku.PremierEntitlements), 3),
			(nameof(CommerceSku.GrowthAllowance), 4),
			(nameof(CommerceSku.TotalEntitlements), 5),
			(nameof(CommerceSku.PreEAConsumption), 6),
			(nameof(CommerceSku.LicenseGenerated), 7),
			(nameof(CommerceSku.LicenseMigrated), 8),
			(nameof(CommerceSku.C1ToDNAMigratedCount), 9),
			(nameof(CommerceSku.TotalConsumption), 10),
			(nameof(CommerceSku.RemainingEntitlements), 11),
			(nameof(CommerceSku.SoftwareDownloads), 12),
			(nameof(CommerceSku.HealthMessage), "h1"),
			(nameof(CommerceSku.CalculationMethod), "cm"),
			(nameof(CommerceSku.CommitmentType), "ct"));
	}

	/// <summary>Every CoverageSummary property is set and read back.</summary>
	[Fact]
	public void CoverageSummary_EveryPropertyRoundTrips()
		=> PropertyAssert.SetAndRead<CoverageSummary>(
			(nameof(CoverageSummary.BasePid), "b"),
			(nameof(CoverageSummary.ContractSiteAddress1), "a1"),
			(nameof(CoverageSummary.ContractSiteCity), "city"),
			(nameof(CoverageSummary.ContractSiteCountry), "us"),
			(nameof(CoverageSummary.ContractSiteCustomerName), "cust"),
			(nameof(CoverageSummary.ContractSiteStateProvince), "ca"),
			(nameof(CoverageSummary.CoveredProductLineEndDate), "2025-01-01"),
			(nameof(CoverageSummary.Id), "i"),
			(nameof(CoverageSummary.IsCovered), "YES"),
			(nameof(CoverageSummary.ItemDescription), "desc"),
			(nameof(CoverageSummary.ItemPosition), "P"),
			(nameof(CoverageSummary.ItemType), "chassis"),
			(nameof(CoverageSummary.LastIndex), 1),
			(nameof(CoverageSummary.OrderablePid), "opid"),
			(nameof(CoverageSummary.PageIndex), 2),
			(nameof(CoverageSummary.PageRecordCount), "10"),
			(nameof(CoverageSummary.ParentSerialNumber), "psn"),
			(nameof(CoverageSummary.PillarCode), "3"),
			(nameof(CoverageSummary.SelfLink), "http://x"),
			(nameof(CoverageSummary.ServiceContractNumber), "scn"),
			(nameof(CoverageSummary.ServiceLineDescription), "sl"),
			(nameof(CoverageSummary.SerialNumber), "sn"),
			(nameof(CoverageSummary.Title), "t"),
			(nameof(CoverageSummary.TotalRecordCount), "100"),
			(nameof(CoverageSummary.WarrantyEndDate), "2026-01-01"),
			(nameof(CoverageSummary.WarrantyType), "w"),
			(nameof(CoverageSummary.WarrantyTypeDescription), "wd"));
}
