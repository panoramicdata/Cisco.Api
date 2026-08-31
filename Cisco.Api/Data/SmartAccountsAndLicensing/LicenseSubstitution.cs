using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing;

/// <summary>
/// Represents the license substitution.
/// </summary>
[DataContract]
public class LicenseSubstitution
{
	/// <summary>
	/// Gets or sets the license.
	/// </summary>
	[DataMember(Name = "license")]
	public string License { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the substituted quantity.
	/// </summary>
	[DataMember(Name = "substitutedQuantity")]
	public int SubstitutedQuantity { get; set; }

	/// <summary>
	/// Gets or sets the substitution type.
	/// </summary>
	[DataMember(Name = "substitutionType")]
	public string SubstitutionType { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the substituted license.
	/// </summary>
	[DataMember(Name = "substitutedLicense")]
	public string SubstitutedLicense { get; set; } = string.Empty;
}
