using Cisco.Api.Data.Psirt;
using Cisco.Api.Data.SecurityAdvisories;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Round-trip test for the SecurityAdvisory data model, otherwise only exercised by integration tests that require live credentials.
/// </summary>
public class SecurityAdvisoryModelTests
{
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
}
