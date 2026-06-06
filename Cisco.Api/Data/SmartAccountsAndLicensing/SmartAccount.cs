using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing;

[DataContract]
[DebuggerDisplay("{Name}")]
public class SmartAccount
{
	[DataMember(Name = "accountStatus")]
	public required string Status { get; set; }

	[DataMember(Name = "accountDomain")]
	public required string Domain { get; set; }

	[DataMember(Name = "accountName")]
	public required string Name { get; set; }

	[DataMember(Name = "accountType")]
	public required SmartAccountType Type { get; set; }
}
