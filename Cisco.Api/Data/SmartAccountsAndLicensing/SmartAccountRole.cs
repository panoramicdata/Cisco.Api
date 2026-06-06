using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing;

public class SmartAccountRole
{
	[DataMember(Name = "role")]
	public required SmartAccountRoleType Type { get; set; }

	[DataMember(Name = "virtualAccount")]
	public string? VirtualAccount { get; set; }
}