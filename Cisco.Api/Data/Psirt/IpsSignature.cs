using System.Runtime.Serialization;

namespace Cisco.Api.Data.Psirt;

[DataContract]
public class IpsSignature
{
	[DataMember(Name = "legacyIpsId")]
	public string LegacyIpsId { get; set; } = null!;

	[DataMember(Name = "releaseVersion")]
	public string ReleaseVersion { get; set; } = null!;

	[DataMember(Name = "softwareVersion")]
	public string SoftwareVersion { get; set; } = null!;

	[DataMember(Name = "legacyIpsUrl")]
	public string LegacyIpsUrl { get; set; } = null!;
}
