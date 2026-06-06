using System.Diagnostics;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing;

[DataContract]
[DebuggerDisplay("{Name}")]
public class SmartAccountWithIds : SmartAccount
{
	[DataMember(Name = "id")]
	public required int Id { get; set; }
}
