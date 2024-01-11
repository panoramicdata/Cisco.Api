using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.Psirt;

[DataContract]
public class AdvisoriesResponse
{
	[DataMember(Name = "advisories")]
	public List<Advisory> Advisories { get; set; } = null!;
}
