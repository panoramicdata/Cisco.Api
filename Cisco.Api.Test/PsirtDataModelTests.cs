using Cisco.Api.Data.Psirt;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Round-trip tests for the Psirt data model.
/// </summary>
public class PsirtDataModelTests
{
	/// <summary>Every Advisory property is set and read back.</summary>
	[Fact]
	public void Advisory_EveryPropertyRoundTrips()
	{
		var advisory = new Advisory();
		advisory.AdvisoryId = "aid";
		advisory.AdvisoryTitle = "t";
		advisory.BugIds = ["b1"];
		advisory.IpsSignatures = [new { sig = "s" }];
		advisory.Cves = ["cve-1"];
		advisory.CvrfUrl = "http://cvrf";
		advisory.CvssBaseScore = "7.0";
		advisory.Cwe = ["cwe-1"];
		advisory.FirstPublished = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);
		advisory.LastUpdated = new DateTimeOffset(2024, 2, 1, 0, 0, 0, TimeSpan.Zero);
		advisory.ProductNames = ["p1"];
		advisory.PublicationUrl = "http://pub";
		advisory.SecurityImpactRating = SecurityImpactRating.High;
		advisory.Summary = "sum";

		advisory.AdvisoryId.Should().Be("aid");
		advisory.AdvisoryTitle.Should().Be("t");
		advisory.BugIds.Should().HaveCount(1);
		advisory.IpsSignatures.Should().HaveCount(1);
		advisory.Cves.Should().HaveCount(1);
		advisory.CvrfUrl.Should().Be("http://cvrf");
		advisory.CvssBaseScore.Should().Be("7.0");
		advisory.Cwe.Should().HaveCount(1);
		advisory.FirstPublished.Should().Be(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero));
		advisory.LastUpdated.Should().Be(new DateTimeOffset(2024, 2, 1, 0, 0, 0, TimeSpan.Zero));
		advisory.ProductNames.Should().HaveCount(1);
		advisory.PublicationUrl.Should().Be("http://pub");
		advisory.SecurityImpactRating.Should().Be(SecurityImpactRating.High);
		advisory.Summary.Should().Be("sum");
	}

	/// <summary>AdvisoriesResponse wraps a list of Advisories.</summary>
	[Fact]
	public void AdvisoriesResponse_WrapsAdvisories()
	{
		var response = new AdvisoriesResponse
		{
			Advisories = [new Advisory { AdvisoryId = "a1" }]
		};
		response.Advisories.Should().HaveCount(1);
		response.Advisories[0].AdvisoryId.Should().Be("a1");
	}
}
