using Cisco.Api.Data.Eox;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Round-trip test for the EoxRecord data model, otherwise only exercised by integration tests that require live credentials.
/// </summary>
public class EoxRecordModelTests
{
	private static CiscoDate DateOn(string value) => new() { Value = value, DateFormat = "yyyy-MM-dd" };

	/// <summary>Every EoxRecord property is set and read back.</summary>
	[Fact]
	public void EoxRecord_EveryPropertyRoundTrips()
		=> PropertyAssert.SetAndRead<EoxRecord>(
			(nameof(EoxRecord.EolProductId), "pid"),
			(nameof(EoxRecord.ProductDescription), "desc"),
			(nameof(EoxRecord.ProductBulletinNumber), "bn"),
			(nameof(EoxRecord.LinkToProductBulletinUrl), "http://x"),
			(nameof(EoxRecord.ExternalAnnouncementDate), DateOn("2024-01-01")),
			(nameof(EoxRecord.EndOfSaleDate), DateOn("2024-02-01")),
			(nameof(EoxRecord.EndOfSoftwareMaintenanceReleases), DateOn("2024-03-01")),
			(nameof(EoxRecord.EndOfSecurityVulnerabilitySupportDate), DateOn("2024-04-01")),
			(nameof(EoxRecord.EndOfRoutineFailureAnalysisDate), DateOn("2024-05-01")),
			(nameof(EoxRecord.EndOfServiceContractRenewalDate), DateOn("2024-06-01")),
			(nameof(EoxRecord.LastSupportDate), DateOn("2024-07-01")),
			(nameof(EoxRecord.EndOfServiceAttachDate), DateOn("2024-08-01")),
			(nameof(EoxRecord.UpdatedDate), DateOn("2024-09-01")),
			(nameof(EoxRecord.Error), new EoxError { ErrorID = "x", ErrorDescription = "y", ErrorDataType = "PRODUCT_ID", ErrorDataValue = "v" }),
			(nameof(EoxRecord.EoxMigrationDetails), new EoxMigrationDetails
			{
				PidActiveFlag = "Y",
				Information = "info",
				Option = "opt",
				ProductId = "pid",
				ProductName = "pname",
				Strategy = "strat",
				ProductInfoUrl = "http://x"
			}),
			(nameof(EoxRecord.InputType), "it"),
			(nameof(EoxRecord.InputValue), "iv"));
}
