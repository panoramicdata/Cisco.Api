using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing;

[DataContract]
public class LicenseSubstitution
{
	[DataMember(Name = "license")]
	public string License { get; set; } = string.Empty;

	[DataMember(Name = "substitutedQuantity")]
	public int SubstitutedQuantity { get; set; }

	[DataMember(Name = "substitutionType")]
	public string SubstitutionType { get; set; } = string.Empty;

	[DataMember(Name = "substitutedLicense")]
	public string SubstitutedLicense { get; set; } = string.Empty;
}
