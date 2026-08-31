using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing;

/// <summary>
/// Defines the supported smart account role type values.
/// </summary>
public enum SmartAccountRoleType
{
	/// <summary>
	/// Represents the account administrator value.
	/// </summary>
	[DataMember(Name = "Smart Account Administrator")]
	AccountAdministrator,

	/// <summary>
	/// Represents the account user value.
	/// </summary>
	[DataMember(Name = "Smart Account User")]
	AccountUser,

	/// <summary>
	/// Represents the account viewer value.
	/// </summary>
	[DataMember(Name = "Smart Account Viewer")]
	AccountViewer,

	//

	/// <summary>
	/// Represents the virtual account administrator value.
	/// </summary>
	[DataMember(Name = "Virtual Account Administrator")]
	VirtualAccountAdministrator,

	/// <summary>
	/// Represents the virtual account user value.
	/// </summary>
	[DataMember(Name = "Virtual Account User")]
	VirtualAccountUser,

	/// <summary>
	/// Represents the virtual account viewer value.
	/// </summary>
	[DataMember(Name = "Virtual Account Viewer")]
	VirtualAccountViewer,
}