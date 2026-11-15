using Cisco.Api.Data.Eox;
using Cisco.Api.Data.ProductInfo;
using Cisco.Api.Data.Psirt;
using Cisco.Api.Data.SecurityAdvisories;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Round-trip tests for three data models whose properties are otherwise only exercised by
/// integration tests that require live credentials.
/// </summary>
public class MoreDataModelRoundTripTests
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

	/// <summary>Every SecurityAdvisory property is set and read back.</summary>
	[Fact]
	public void SecurityAdvisory_EveryPropertyRoundTrips()
		=> PropertyAssert.SetAndRead<SecurityAdvisory>(
			(nameof(SecurityAdvisory.AdvisoryId), "aid"),
			(nameof(SecurityAdvisory.AdvisoryTitle), "t"),
			(nameof(SecurityAdvisory.BugIds), new List<string> { "b1", "b2" }),
			(nameof(SecurityAdvisory.Cves), new List<string> { "cve-1" }),
			(nameof(SecurityAdvisory.Platforms), new List<Platform> { new() { Id = "1", Name = "n" } }),
			(nameof(SecurityAdvisory.CvrfUrl), "http://cvrf"),
			(nameof(SecurityAdvisory.CsafUrl), "http://csaf"),
			(nameof(SecurityAdvisory.CvssBaseScore), "7.0"),
			(nameof(SecurityAdvisory.Cwes), new List<string> { "cwe-1" }),
			(nameof(SecurityAdvisory.FirstFixed), new List<string> { "ff1" }),
			(nameof(SecurityAdvisory.IpsSignatures), new { sig = "s" }),
			(nameof(SecurityAdvisory.IosReleases), new List<string> { "ios-1" }),
			(nameof(SecurityAdvisory.FirstPublished), new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero)),
			(nameof(SecurityAdvisory.LastUpdated), new DateTimeOffset(2024, 2, 1, 0, 0, 0, TimeSpan.Zero)),
			(nameof(SecurityAdvisory.Status), "ok"),
			(nameof(SecurityAdvisory.Version), "v1"),
			(nameof(SecurityAdvisory.ProductNames), new List<string> { "p1" }),
			(nameof(SecurityAdvisory.PublicationUrl), "http://pub"),
			(nameof(SecurityAdvisory.SecurityImpactRating), SecurityImpactRating.High),
			(nameof(SecurityAdvisory.Summary), "sum"));

	/// <summary>Every ProductInformation property is set and read back.</summary>
	[Fact]
	public void ProductInformation_EveryPropertyRoundTrips()
		=> PropertyAssert.SetAndRead<ProductInformation>(
			(nameof(ProductInformation.Dimensions), new Dimensions { Format = "10x10x10", Value = "cm" }),
			(nameof(ProductInformation.FormFactor), "ff"),
			(nameof(ProductInformation.Id), "i"),
			(nameof(ProductInformation.BasePid), "b"),
			(nameof(ProductInformation.OrderablePid), "o"),
			(nameof(ProductInformation.OrderableStatus), "O"),
			(nameof(ProductInformation.ProductCategory), "cat"),
			(nameof(ProductInformation.ProductName), "n"),
			(nameof(ProductInformation.ProductSeries), "s"),
			(nameof(ProductInformation.ProductSubcategory), "sc"),
			(nameof(ProductInformation.ProductSupportPage), "http://x"),
			(nameof(ProductInformation.ProductType), "t"),
			(nameof(ProductInformation.ReleaseDate), "2024-01-01"),
			(nameof(ProductInformation.RichMediaUrls), new RichMediaUrls { SmallImageUrl = "s1", LargeImageUrl = "l1" }),
			(nameof(ProductInformation.SerialNumber), "sn"),
			(nameof(ProductInformation.VisioStencilUrl), "http://v"),
			(nameof(ProductInformation.Weight), "1kg"),
			(nameof(ProductInformation.ErrorResponse), new ErrorResponse
			{
				ApiError = new ApiError { ErrorCode = "E", ErrorDescription = "d", SuggestedAction = "s" }
			}));
}
