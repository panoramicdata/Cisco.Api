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
	/// <summary>Every EoxRecord property is set and read back.</summary>
	[Fact]
	public void EoxRecord_EveryPropertyRoundTrips()
	{
		var record = new EoxRecord();
		record.EolProductId = "pid";
		record.ProductDescription = "desc";
		record.ProductBulletinNumber = "bn";
		record.LinkToProductBulletinUrl = "http://x";
		record.ExternalAnnouncementDate = new CiscoDate { Value = "2024-01-01", DateFormat = "yyyy-MM-dd" };
		record.EndOfSaleDate = new CiscoDate { Value = "2024-02-01", DateFormat = "yyyy-MM-dd" };
		record.EndOfSoftwareMaintenanceReleases = new CiscoDate { Value = "2024-03-01", DateFormat = "yyyy-MM-dd" };
		record.EndOfSecurityVulnerabilitySupportDate = new CiscoDate { Value = "2024-04-01", DateFormat = "yyyy-MM-dd" };
		record.EndOfRoutineFailureAnalysisDate = new CiscoDate { Value = "2024-05-01", DateFormat = "yyyy-MM-dd" };
		record.EndOfServiceContractRenewalDate = new CiscoDate { Value = "2024-06-01", DateFormat = "yyyy-MM-dd" };
		record.LastSupportDate = new CiscoDate { Value = "2024-07-01", DateFormat = "yyyy-MM-dd" };
		record.EndOfServiceAttachDate = new CiscoDate { Value = "2024-08-01", DateFormat = "yyyy-MM-dd" };
		record.UpdatedDate = new CiscoDate { Value = "2024-09-01", DateFormat = "yyyy-MM-dd" };
		record.Error = new EoxError { ErrorID = "x", ErrorDescription = "y", ErrorDataType = "PRODUCT_ID", ErrorDataValue = "v" };
		record.EoxMigrationDetails = new EoxMigrationDetails
		{
			PidActiveFlag = "Y",
			Information = "info",
			Option = "opt",
			ProductId = "pid",
			ProductName = "pname",
			Strategy = "strat",
			ProductInfoUrl = "http://x"
		};
		record.InputType = "it";
		record.InputValue = "iv";

		record.EolProductId.Should().Be("pid");
		record.ProductDescription.Should().Be("desc");
		record.ProductBulletinNumber.Should().Be("bn");
		record.LinkToProductBulletinUrl.Should().Be("http://x");
		record.ExternalAnnouncementDate.Should().NotBeNull();
		record.EndOfSaleDate.Should().NotBeNull();
		record.EndOfSoftwareMaintenanceReleases.Should().NotBeNull();
		record.EndOfSecurityVulnerabilitySupportDate.Should().NotBeNull();
		record.EndOfRoutineFailureAnalysisDate.Should().NotBeNull();
		record.EndOfServiceContractRenewalDate.Should().NotBeNull();
		record.LastSupportDate.Should().NotBeNull();
		record.EndOfServiceAttachDate.Should().NotBeNull();
		record.UpdatedDate.Should().NotBeNull();
		record.Error.Should().NotBeNull();
		record.EoxMigrationDetails.Should().NotBeNull();
		record.InputType.Should().Be("it");
		record.InputValue.Should().Be("iv");
	}

	/// <summary>Every SecurityAdvisory property is set and read back.</summary>
	[Fact]
	public void SecurityAdvisory_EveryPropertyRoundTrips()
	{
		var advisory = new SecurityAdvisory();
		advisory.AdvisoryId = "aid";
		advisory.AdvisoryTitle = "t";
		advisory.BugIds = ["b1", "b2"];
		advisory.Cves = ["cve-1"];
		advisory.Platforms = [new Platform { Id = "1", Name = "n" }];
		advisory.CvrfUrl = "http://cvrf";
		advisory.CsafUrl = "http://csaf";
		advisory.CvssBaseScore = "7.0";
		advisory.Cwes = ["cwe-1"];
		advisory.FirstFixed = ["ff1"];
		advisory.IpsSignatures = new { sig = "s" };
		advisory.IosReleases = ["ios-1"];
		advisory.FirstPublished = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);
		advisory.LastUpdated = new DateTimeOffset(2024, 2, 1, 0, 0, 0, TimeSpan.Zero);
		advisory.Status = "ok";
		advisory.Version = "v1";
		advisory.ProductNames = ["p1"];
		advisory.PublicationUrl = "http://pub";
		advisory.SecurityImpactRating = SecurityImpactRating.High;
		advisory.Summary = "sum";

		advisory.AdvisoryId.Should().Be("aid");
		advisory.AdvisoryTitle.Should().Be("t");
		advisory.BugIds.Should().HaveCount(2);
		advisory.Cves.Should().HaveCount(1);
		advisory.Platforms.Should().HaveCount(1);
		advisory.CvrfUrl.Should().Be("http://cvrf");
		advisory.CsafUrl.Should().Be("http://csaf");
		advisory.CvssBaseScore.Should().Be("7.0");
		advisory.Cwes.Should().HaveCount(1);
		advisory.FirstFixed.Should().HaveCount(1);
		advisory.IpsSignatures.Should().NotBeNull();
		advisory.IosReleases.Should().HaveCount(1);
		advisory.FirstPublished.Should().Be(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero));
		advisory.LastUpdated.Should().Be(new DateTimeOffset(2024, 2, 1, 0, 0, 0, TimeSpan.Zero));
		advisory.Status.Should().Be("ok");
		advisory.Version.Should().Be("v1");
		advisory.ProductNames.Should().HaveCount(1);
		advisory.PublicationUrl.Should().Be("http://pub");
		advisory.SecurityImpactRating.Should().Be(SecurityImpactRating.High);
		advisory.Summary.Should().Be("sum");
	}

	/// <summary>Every ProductInformation property is set and read back.</summary>
	[Fact]
	public void ProductInformation_EveryPropertyRoundTrips()
	{
		var info = new ProductInformation();
		info.Dimensions = new Dimensions { Format = "10x10x10", Value = "cm" };
		info.FormFactor = "ff";
		info.Id = "i";
		info.BasePid = "b";
		info.OrderablePid = "o";
		info.OrderableStatus = "O";
		info.ProductCategory = "cat";
		info.ProductName = "n";
		info.ProductSeries = "s";
		info.ProductSubcategory = "sc";
		info.ProductSupportPage = "http://x";
		info.ProductType = "t";
		info.ReleaseDate = "2024-01-01";
		info.RichMediaUrls = new RichMediaUrls { SmallImageUrl = "s1", LargeImageUrl = "l1" };
		info.SerialNumber = "sn";
		info.VisioStencilUrl = "http://v";
		info.Weight = "1kg";
		info.ErrorResponse = new ErrorResponse
		{
			ApiError = new ApiError
			{
				ErrorCode = "E",
				ErrorDescription = "d",
				SuggestedAction = "s"
			}
		};

		info.Dimensions.Should().NotBeNull();
		info.FormFactor.Should().Be("ff");
		info.Id.Should().Be("i");
		info.BasePid.Should().Be("b");
		info.OrderablePid.Should().Be("o");
		info.OrderableStatus.Should().Be("O");
		info.ProductCategory.Should().Be("cat");
		info.ProductName.Should().Be("n");
		info.ProductSeries.Should().Be("s");
		info.ProductSubcategory.Should().Be("sc");
		info.ProductSupportPage.Should().Be("http://x");
		info.ProductType.Should().Be("t");
		info.ReleaseDate.Should().Be("2024-01-01");
		info.RichMediaUrls.Should().NotBeNull();
		info.SerialNumber.Should().Be("sn");
		info.VisioStencilUrl.Should().Be("http://v");
		info.Weight.Should().Be("1kg");
		info.ErrorResponse.Should().NotBeNull();
	}
}
