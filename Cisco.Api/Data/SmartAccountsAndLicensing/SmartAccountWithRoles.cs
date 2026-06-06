using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing;

[DataContract]
[DebuggerDisplay("{Name}")]
public class SmartAccountWithRoles : SmartAccount
{
	[DataMember(Name = "roles")]
	public required List<SmartAccountRole> Roles { get; set; }
}
