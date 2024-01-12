using Cisco.Api.Data.Shared;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SecurityAdvisories;

[DataContract]
public class Platform : NamedIdentifiedItem
{
	[DataMember(Name = "firstFixes")]
	public List<FirstFix> FirstFixes { get; set; } = [];

	[DataMember(Name = "vulnerabilityState")]
	public string VulnerabilityState { get; set; } = string.Empty;
}