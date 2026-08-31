using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing;

/// <summary>
/// Represents the smart account role.
/// </summary>
public class SmartAccountRole
{
	/// <summary>
	/// Gets or sets the type.
	/// </summary>
	[DataMember(Name = "role")]
	public required SmartAccountRoleType Type { get; set; }

	/// <summary>
	/// Gets or sets the virtual account.
	/// </summary>
	[DataMember(Name = "virtualAccount")]
	public string? VirtualAccount { get; set; }
}