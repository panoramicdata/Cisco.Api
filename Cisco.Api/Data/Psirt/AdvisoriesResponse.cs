using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.Psirt;

/// <summary>
/// Represents the advisories response.
/// </summary>
[DataContract]
public class AdvisoriesResponse
{
	/// <summary>
	/// Gets or sets the advisories.
	/// </summary>
	[DataMember(Name = "advisories")]
	public List<Advisory> Advisories { get; set; } = null!;
}
