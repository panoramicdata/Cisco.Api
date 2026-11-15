using System.Runtime.Serialization;

namespace Cisco.Api.Data.EnterpriseAgreement.Responses;

/// <summary>
/// Represents the commerce SKU.
/// </summary>
[DataContract]
public class CommerceSku : SuiteEntitlements
{
	/// <summary>
	/// Gets or sets the EOL flag.
	/// </summary>
	[DataMember(Name = "eol")]
	public bool Eol { get; set; }

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
	/// Gets or sets the EOL message.
	/// </summary>
	[DataMember(Name = "eolMessage")]
	public string EolMessage { get; set; } = string.Empty;

	/// <summary>
	/// Creates a new <see cref="CommerceSku"/> from the specified values.
	/// </summary>
	/// <param name="eol">The EOL flag.</param>
	/// <param name="custSuiteId">The cust suite ID.</param>
	/// <param name="suiteCommerceSku">The suite commerce SKU.</param>
	/// <param name="suiteCommerceSkuDescription">The suite commerce SKU description.</param>
	/// <param name="suiteName">The suite name.</param>
	/// <param name="custSuiteName">The cust suite name.</param>
	/// <param name="eolMessage">The EOL message.</param>
	/// <param name="purchasedEntitlements">The purchased entitlements.</param>
	/// <param name="premierEntitlements">The premier entitlements.</param>
	/// <param name="growthAllowance">The growth allowance.</param>
	/// <param name="totalEntitlements">The total entitlements.</param>
	/// <param name="preEAConsumption">The pre EA consumption.</param>
	/// <param name="licenseGenerated">The license generated.</param>
	/// <param name="licenseMigrated">The license migrated.</param>
	/// <param name="c1ToDNAMigratedCount">The C1 to DNA migrated count.</param>
	/// <param name="totalConsumption">The total consumption.</param>
	/// <param name="remainingEntitlements">The remaining entitlements.</param>
	/// <param name="softwareDownloads">The software downloads.</param>
	/// <param name="healthMessage">The health message.</param>
	/// <param name="calculationMethod">The calculation method.</param>
	/// <param name="commitmentType">The commitment type.</param>
	/// <returns>A new <see cref="CommerceSku"/> instance.</returns>
	public static CommerceSku Create(
		bool eol,
		int custSuiteId,
		string suiteCommerceSku,
		string suiteCommerceSkuDescription,
		string suiteName,
		string custSuiteName,
		string eolMessage,
		int purchasedEntitlements,
		int premierEntitlements,
		int growthAllowance,
		int totalEntitlements,
		int preEAConsumption,
		int licenseGenerated,
		int licenseMigrated,
		int c1ToDNAMigratedCount,
		int totalConsumption,
		int remainingEntitlements,
		int softwareDownloads,
		string healthMessage,
		string calculationMethod,
		string commitmentType)
	{
		return new CommerceSku
		{
			Eol = eol,
			CustSuiteId = custSuiteId,
			SuiteCommerceSku = suiteCommerceSku,
			SuiteCommerceSkuDescription = suiteCommerceSkuDescription,
			SuiteName = suiteName,
			CustSuiteName = custSuiteName,
			EolMessage = eolMessage,
			PurchasedEntitlements = purchasedEntitlements,
			PremierEntitlements = premierEntitlements,
			GrowthAllowance = growthAllowance,
			TotalEntitlements = totalEntitlements,
			PreEAConsumption = preEAConsumption,
			LicenseGenerated = licenseGenerated,
			LicenseMigrated = licenseMigrated,
			C1ToDNAMigratedCount = c1ToDNAMigratedCount,
			TotalConsumption = totalConsumption,
			RemainingEntitlements = remainingEntitlements,
			SoftwareDownloads = softwareDownloads,
			HealthMessage = healthMessage,
			CalculationMethod = calculationMethod,
			CommitmentType = commitmentType,
		};
	}
}
