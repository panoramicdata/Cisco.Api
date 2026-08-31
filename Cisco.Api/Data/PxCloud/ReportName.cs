using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

/// <summary>
/// Defines the supported report name values.
/// </summary>
[DataContract]
public enum ReportName
{
	/// <summary>
	/// Represents the assets value.
	/// </summary>
	[EnumMember(Value = "Assets")]
	Assets,

	/// <summary>
	/// Represents the field notices value.
	/// </summary>
	[EnumMember(Value = "FieldNotices")]
	FieldNotices,

	/// <summary>
	/// Represents the hardware value.
	/// </summary>
	[EnumMember(Value = "Hardware")]
	Hardware,

	/// <summary>
	/// Represents the licenses value.
	/// </summary>
	[EnumMember(Value = "Licenses")]
	Licenses,

	/// <summary>
	/// Represents the purchased licenses value.
	/// </summary>
	[EnumMember(Value = "PurchasedLicenses")]
	PurchasedLicenses,

	/// <summary>
	/// Represents the security advisories value.
	/// </summary>
	[EnumMember(Value = "SecurityAdvisories")]
	SecurityAdvisories,

	/// <summary>
	/// Represents the software value.
	/// </summary>
	[EnumMember(Value = "Software")]
	Software,

	/// <summary>
	/// Represents the priority bugs value.
	/// </summary>
	[EnumMember(Value = "PriorityBugs")]
	PriorityBugs
};
