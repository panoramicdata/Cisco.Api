using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SecurityAdvisories;

[DataContract]
public class SecurityAdvisory
{
	[DataMember(Name = "advisoryId")]
	public string AdvisoryId { get; set; } = string.Empty;

	[DataMember(Name = "advisoryTitle")]
	public string AdvisoryTitle { get; set; } = string.Empty;

	[DataMember(Name = "bugIDs")]
	public List<string> BugIds { get; set; } = [];

	[DataMember(Name = "cves")]
	public List<string> Cves { get; set; } = [];

	[DataMember(Name = "platforms")]
	public List<Platform> Platforms { get; set; } = [];

	[DataMember(Name = "cvrfUrl")]
	public string CvrfUrl { get; set; } = string.Empty;

	[DataMember(Name = "csafUrl")]
	public string CsafUrl { get; set; } = string.Empty;

	[DataMember(Name = "cvssBaseScore")]
	public string CvssBaseScore { get; set; } = string.Empty;

	[DataMember(Name = "cwe")]
	public List<string> Cwes { get; set; } = [];

	[DataMember(Name = "firstFixed")]
	public List<string> FirstFixed { get; set; } = [];

	[DataMember(Name = "ipsSignatures")]
	public List<string> IpsSignatures { get; set; } = [];

	[DataMember(Name = "iosRelease")]
	public List<string> IosReleases { get; set; } = [];

	[DataMember(Name = "firstPublished")]
	public string FirstPublished { get; set; } = string.Empty;

	[DataMember(Name = "lastUpdated")]
	public string LastUpdated { get; set; } = string.Empty;

	[DataMember(Name = "status")]
	public string Status { get; set; } = string.Empty;

	[DataMember(Name = "version")]
	public string Version { get; set; } = string.Empty;

	[DataMember(Name = "productNames")]
	public List<string> ProductNames { get; set; } = [];

	[DataMember(Name = "publicationUrl")]
	public string PublicationUrl { get; set; } = string.Empty;

	[DataMember(Name = "sir")]
	public string Sir { get; set; } = string.Empty;

	[DataMember(Name = "summary")]
	public string Summary { get; set; } = string.Empty;
}