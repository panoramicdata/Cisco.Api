using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public enum ReportName
{
	[EnumMember(Value = "Assets")]
	Assets,

	[EnumMember(Value = "FieldNotices")]
	FieldNotices,

	[EnumMember(Value = "Hardware")]
	Hardware,

	[EnumMember(Value = "Licenses")]
	Licenses,

	[EnumMember(Value = "PurchasedLicenses")]
	PurchasedLicenses,

	[EnumMember(Value = "SecurityAdvisories")]
	SecurityAdvisories,

	[EnumMember(Value = "Software")]
	Software,

	[EnumMember(Value = "PriorityBugs")]
	PriorityBugs
};
