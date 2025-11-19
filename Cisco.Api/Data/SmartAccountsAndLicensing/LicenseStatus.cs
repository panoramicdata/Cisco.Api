using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing;

public enum LicenseStatus
{
	[DataMember(Name = "Active")]
	Active,

	[DataMember(Name = "Cancelled")]
	Canceled,

	[DataMember(Name = "Expired")]
	Expired,
}