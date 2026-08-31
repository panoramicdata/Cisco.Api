using Cisco.Api.Data.Shared;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SecurityAdvisories;

/// <summary>
/// Represents the platform.
/// </summary>
[DataContract]
public class Platform : NamedIdentifiedItem
{
	/// <summary>
	/// Gets or sets the first fixes.
	/// </summary>
	[DataMember(Name = "firstFixes")]
	public List<FirstFix> FirstFixes { get; set; } = [];

	/// <summary>
	/// Gets or sets the vulnerability state.
	/// </summary>
	[DataMember(Name = "vulnerabilityState")]
	public string VulnerabilityState { get; set; } = string.Empty;
}