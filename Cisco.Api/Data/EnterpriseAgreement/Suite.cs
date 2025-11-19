using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.EnterpriseAgreement.Responses;

[DataContract]
public class Suite
{
	[DataMember(Name = "custSuiteId")]
	public int CustSuiteId { get; set; }

	[DataMember(Name = "suiteName")]
	public string SuiteName { get; set; } = string.Empty;

	[DataMember(Name = "custSuiteName")]
	public string CustSuiteName { get; set; } = string.Empty;

	[DataMember(Name = "purchasedEntitlements")]
	public int PurchasedEntitlements { get; set; }

	[DataMember(Name = "premierEntitlements")]
	public int PremierEntitlements { get; set; }

	[DataMember(Name = "growthAllowance")]
	public int GrowthAllowance { get; set; }

	[DataMember(Name = "totalEntitlements")]
	public int TotalEntitlements { get; set; }

	[DataMember(Name = "preEAConsumption")]
	public int PreEAConsumption { get; set; }

	[DataMember(Name = "licenseGenerated")]
	public int LicenseGenerated { get; set; }

	[DataMember(Name = "licenseMigrated")]
	public int LicenseMigrated { get; set; }

	[DataMember(Name = "c1ToDNAMigratedCount")]
	public int C1ToDNAMigratedCount { get; set; }

	[DataMember(Name = "totalConsumption")]
	public int TotalConsumption { get; set; }

	[DataMember(Name = "remainingEntitlements")]
	public int RemainingEntitlements { get; set; }

	[DataMember(Name = "softwareDownloads")]
	public int SoftwareDownloads { get; set; }

	[DataMember(Name = "healthMessage")]
	public string HealthMessage { get; set; } = string.Empty;

	[DataMember(Name = "calculationMethod")]
	public string CalculationMethod { get; set; } = string.Empty;

	[DataMember(Name = "commitmentType")]
	public string CommitmentType { get; set; } = string.Empty;

	[DataMember(Name = "commerceSkus")]
	public List<CommerceSku> CommerceSkus { get; set; } = [];
}
