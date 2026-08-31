using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing;

/// <summary>
/// Represents the smart account with roles.
/// </summary>
[DataContract]
[DebuggerDisplay("{Name}")]
public class SmartAccountWithRoles : SmartAccount
{
	/// <summary>
	/// Gets or sets the roles.
	/// </summary>
	[DataMember(Name = "roles")]
	public required List<SmartAccountRole> Roles { get; set; }
}
