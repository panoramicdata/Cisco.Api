using System.Runtime.Serialization;

namespace Cisco.Api.Data.EnterpriseAgreement.Responses;

/// <summary>
/// Represents the commerce SKU.
/// </summary>
[DataContract]
public class CommerceSku
{
	/// <summary>
	/// Gets or sets the EOL flag.
	/// </summary>
	[DataMember(Name = "eol")]
	public bool Eol { get; set; }

	/// <summary>
	/// Gets or sets the cust suite ID.
	/// </summary>
	[DataMember(Name = "custSuiteId")]
	public int CustSuiteId { get; set; }

	/// <summary>
	/// Gets or sets the suite commerce SKU.
	/// </summary>
	[DataMember(Name = "commerceSku")]
	public string SuiteCommerceSku { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the suite commerce SKU description.
	/// </summary>
	[DataMember(Name = "commerceSkuDescription")]
	public string SuiteCommerceSkuDescription { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the suite name.
	/// </summary>
	[DataMember(Name = "suiteName")]
	public string SuiteName { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the cust suite name.
	/// </summary>
	[DataMember(Name = "custSuiteName")]
	public string CustSuiteName { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the EOL message.
	/// </summary>
	[DataMember(Name = "eolMessage")]
	public string EolMessage { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the purchased entitlements.
	/// </summary>
	[DataMember(Name = "purchasedEntitlements")]
	public int PurchasedEntitlements { get; set; }

	/// <summary>
	/// Gets or sets the premier entitlements.
	/// </summary>
	[DataMember(Name = "premierEntitlements")]
	public int PremierEntitlements { get; set; }

	/// <summary>
	/// Gets or sets the growth allowance.
	/// </summary>
	[DataMember(Name = "growthAllowance")]
	public int GrowthAllowance { get; set; }

	/// <summary>
	/// Gets or sets the total entitlements.
	/// </summary>
	[DataMember(Name = "totalEntitlements")]
	public int TotalEntitlements { get; set; }

	/// <summary>
	/// Gets or sets the pre EA consumption.
	/// </summary>
	[DataMember(Name = "preEAConsumption")]
	public int PreEAConsumption { get; set; }

	/// <summary>
	/// Gets or sets the license generated.
	/// </summary>
	[DataMember(Name = "licenseGenerated")]
	public int LicenseGenerated { get; set; }

	/// <summary>
	/// Gets or sets the license migrated.
	/// </summary>
	[DataMember(Name = "licenseMigrated")]
	public int LicenseMigrated { get; set; }

	/// <summary>
	/// Gets or sets the C1 to DNA migrated count.
	/// </summary>
	[DataMember(Name = "c1ToDNAMigratedCount")]
	public int C1ToDNAMigratedCount { get; set; }

	/// <summary>
	/// Gets or sets the total consumption.
	/// </summary>
	[DataMember(Name = "totalConsumption")]
	public int TotalConsumption { get; set; }

	/// <summary>
	/// Gets or sets the remaining entitlements.
	/// </summary>
	[DataMember(Name = "remainingEntitlements")]
	public int RemainingEntitlements { get; set; }

	/// <summary>
	/// Gets or sets the software downloads.
	/// </summary>
	[DataMember(Name = "softwareDownloads")]
	public int SoftwareDownloads { get; set; }

	/// <summary>
	/// Gets or sets the health message.
	/// </summary>
	[DataMember(Name = "healthMessage")]
	public string HealthMessage { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the calculation method.
	/// </summary>
	[DataMember(Name = "calculationMethod")]
	public string CalculationMethod { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the commitment type.
	/// </summary>
	[DataMember(Name = "commitmentType")]
	public string CommitmentType { get; set; } = string.Empty;
}
