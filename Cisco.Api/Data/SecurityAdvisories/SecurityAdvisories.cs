using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SecurityAdvisories;

[DataContract]
public class SecurityAdvisories
{
	[DataMember(Name = "advisories")]
	public List<SecurityAdvisory> Advisories { get; set; } = [];
}