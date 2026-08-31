using Cisco.Api.Data.Psirt;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SecurityAdvisories;

/// <summary>
/// Represents the security advisory.
/// </summary>
[DataContract]
public class SecurityAdvisory
{
	/// <summary>
	/// Gets or sets the advisory ID.
	/// </summary>
	[DataMember(Name = "advisoryId")]
	public string AdvisoryId { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the advisory title.
	/// </summary>
	[DataMember(Name = "advisoryTitle")]
	public string AdvisoryTitle { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the bug ids.
	/// </summary>
	[DataMember(Name = "bugIDs")]
	public List<string> BugIds { get; set; } = [];

	/// <summary>
	/// Gets or sets the cves.
	/// </summary>
	[DataMember(Name = "cves")]
	public List<string> Cves { get; set; } = [];

	/// <summary>
	/// Gets or sets the platforms.
	/// </summary>
	[DataMember(Name = "platforms")]
	public List<Platform> Platforms { get; set; } = [];

	/// <summary>
	/// Gets or sets the cvrf URL.
	/// </summary>
	[DataMember(Name = "cvrfUrl")]
	public string CvrfUrl { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the csaf URL.
	/// </summary>
	[DataMember(Name = "csafUrl")]
	public string CsafUrl { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the CVSS base score.
	/// </summary>
	[DataMember(Name = "cvssBaseScore")]
	public string CvssBaseScore { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the cwes.
	/// </summary>
	[DataMember(Name = "cwe")]
	public List<string> Cwes { get; set; } = [];

	/// <summary>
	/// Gets or sets the first fixed.
	/// </summary>
	[DataMember(Name = "firstFixed")]
	public List<string> FirstFixed { get; set; } = [];

	/// <summary>
	/// Gets or sets the ips signatures.
	/// </summary>
	[DataMember(Name = "ipsSignatures")]
	public object? IpsSignatures { get; set; }

	/// <summary>
	/// Gets or sets the ios releases.
	/// </summary>
	[DataMember(Name = "iosRelease")]
	public List<string> IosReleases { get; set; } = [];

	/// <summary>
	/// Gets or sets the first published.
	/// </summary>
	[DataMember(Name = "firstPublished")]
	public DateTimeOffset FirstPublished { get; set; }

	/// <summary>
	/// Gets or sets the last updated.
	/// </summary>
	[DataMember(Name = "lastUpdated")]
	public DateTimeOffset LastUpdated { get; set; }

	/// <summary>
	/// Gets or sets the status.
	/// </summary>
	[DataMember(Name = "status")]
	public string Status { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the version.
	/// </summary>
	[DataMember(Name = "version")]
	public string Version { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the product names.
	/// </summary>
	[DataMember(Name = "productNames")]
	public List<string> ProductNames { get; set; } = [];

	/// <summary>
	/// Gets or sets the publication URL.
	/// </summary>
	[DataMember(Name = "publicationUrl")]
	public string PublicationUrl { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the security impact rating.
	/// </summary>
	[DataMember(Name = "sir")]
	public SecurityImpactRating SecurityImpactRating { get; set; }

	/// <summary>
	/// Gets or sets the summary.
	/// </summary>
	[DataMember(Name = "summary")]
	public string Summary { get; set; } = string.Empty;
}