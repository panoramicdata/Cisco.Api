using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.Psirt;

/// <summary>
/// Represents the advisory.
/// </summary>
[DataContract]
public class Advisory
{
	/// <summary>
	/// Gets or sets the advisory ID.
	/// </summary>
	[DataMember(Name = "advisoryId")]
	public string AdvisoryId { get; set; } = null!;

	/// <summary>
	/// Gets or sets the advisory title.
	/// </summary>
	[DataMember(Name = "advisoryTitle")]
	public string AdvisoryTitle { get; set; } = null!;

	/// <summary>
	/// Gets or sets the bug ids.
	/// </summary>
	[DataMember(Name = "bugIDs")]
	public List<string> BugIds { get; set; } = null!;

	/// <summary>
	/// Gets or sets the ips signatures.
	/// </summary>
	[DataMember(Name = "ipsSignatures")]
	public List<object> IpsSignatures { get; set; } = null!;

	/// <summary>
	/// Gets or sets the cves.
	/// </summary>
	[DataMember(Name = "cves")]
	public List<string> Cves { get; set; } = null!;

	/// <summary>
	/// Gets or sets the cvrf URL.
	/// </summary>
	[DataMember(Name = "cvrfUrl")]
	public string CvrfUrl { get; set; } = null!;

	/// <summary>
	/// Gets or sets the CVSS base score.
	/// </summary>
	[DataMember(Name = "cvssBaseScore")]
	public string CvssBaseScore { get; set; } = null!;

	/// <summary>
	/// Gets or sets the CWE.
	/// </summary>
	[DataMember(Name = "cwe")]
	public List<string> Cwe { get; set; } = null!;

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
	/// Gets or sets the product names.
	/// </summary>
	[DataMember(Name = "productNames")]
	public List<string> ProductNames { get; set; } = null!;

	/// <summary>
	/// Gets or sets the publication URL.
	/// </summary>
	[DataMember(Name = "publicationUrl")]
	public string PublicationUrl { get; set; } = null!;

	/// <summary>
	/// Gets or sets the security impact rating.
	/// </summary>
	[DataMember(Name = "sir")]
	public SecurityImpactRating SecurityImpactRating { get; set; }

	/// <summary>
	/// Gets or sets the summary.
	/// </summary>
	[DataMember(Name = "summary")]
	public string Summary { get; set; } = null!;
}
