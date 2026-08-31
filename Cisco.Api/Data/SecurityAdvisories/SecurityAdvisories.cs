using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SecurityAdvisories;

/// <summary>
/// Represents the security advisories.
/// </summary>
[DataContract]
public class SecurityAdvisories
{
	/// <summary>
	/// Gets or sets the advisories.
	/// </summary>
	[DataMember(Name = "advisories")]
	public List<SecurityAdvisory> Advisories { get; set; } = [];
}