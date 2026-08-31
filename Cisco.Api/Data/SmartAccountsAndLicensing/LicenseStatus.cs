using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing;

/// <summary>
/// Defines the supported license status values.
/// </summary>
public enum LicenseStatus
{
	/// <summary>
	/// Represents the active value.
	/// </summary>
	[DataMember(Name = "Active")]
	Active,

	/// <summary>
	/// Represents the canceled value.
	/// </summary>
	[DataMember(Name = "Cancelled")]
	Canceled,

	/// <summary>
	/// Represents the expired value.
	/// </summary>
	[DataMember(Name = "Expired")]
	Expired,

	/// <summary>
	/// Represents the future value.
	/// </summary>
	[DataMember(Name = "Future")]
	Future,

	/// <summary>
	/// Represents the pending value.
	/// </summary>
	[DataMember(Name = "Pending")]
	Pending,
}