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
		=> PropertyAssert.SetAndRead<Advisory>(
			(nameof(Advisory.AdvisoryId), "aid"),
			(nameof(Advisory.AdvisoryTitle), "t"),
			(nameof(Advisory.BugIds), new List<string> { "b1" }),
			(nameof(Advisory.IpsSignatures), new List<object> { new { sig = "s" } }),
			(nameof(Advisory.Cves), new List<string> { "cve-1" }),
			(nameof(Advisory.CvrfUrl), "http://cvrf"),
			(nameof(Advisory.CvssBaseScore), "7.0"),
			(nameof(Advisory.Cwe), new List<string> { "cwe-1" }),
			(nameof(Advisory.FirstPublished), new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero)),
			(nameof(Advisory.LastUpdated), new DateTimeOffset(2024, 2, 1, 0, 0, 0, TimeSpan.Zero)),
			(nameof(Advisory.ProductNames), new List<string> { "p1" }),
			(nameof(Advisory.PublicationUrl), "http://pub"),
			(nameof(Advisory.SecurityImpactRating), SecurityImpactRating.High),
			(nameof(Advisory.Summary), "sum"));

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
