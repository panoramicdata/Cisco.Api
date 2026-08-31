using System.Runtime.Serialization;

namespace Cisco.Api.Data.Psirt;

/// <summary>
/// Represents the ips signature.
/// </summary>
[DataContract]
public class IpsSignature
{
	/// <summary>
	/// Gets or sets the legacy ips ID.
	/// </summary>
	[DataMember(Name = "legacyIpsId")]
	public string LegacyIpsId { get; set; } = null!;

	/// <summary>
	/// Gets or sets the release version.
	/// </summary>
	[DataMember(Name = "releaseVersion")]
	public string ReleaseVersion { get; set; } = null!;

	/// <summary>
	/// Gets or sets the software version.
	/// </summary>
	[DataMember(Name = "softwareVersion")]
	public string SoftwareVersion { get; set; } = null!;

	/// <summary>
	/// Gets or sets the legacy ips URL.
	/// </summary>
	[DataMember(Name = "legacyIpsUrl")]
	public string LegacyIpsUrl { get; set; } = null!;
}
