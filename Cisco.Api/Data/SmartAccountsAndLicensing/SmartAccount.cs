using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing;

/// <summary>
/// Represents the smart account.
/// </summary>
[DataContract]
[DebuggerDisplay("{Name}")]
public class SmartAccount
{
	/// <summary>
	/// Gets or sets the status.
	/// </summary>
	[DataMember(Name = "accountStatus")]
	public required string Status { get; set; }

	/// <summary>
	/// Gets or sets the domain.
	/// </summary>
	[DataMember(Name = "accountDomain")]
	public required string Domain { get; set; }

	/// <summary>
	/// Gets or sets the name.
	/// </summary>
	[DataMember(Name = "accountName")]
	public required string Name { get; set; }

	/// <summary>
	/// Gets or sets the type.
	/// </summary>
	[DataMember(Name = "accountType")]
	public required SmartAccountType Type { get; set; }
}
