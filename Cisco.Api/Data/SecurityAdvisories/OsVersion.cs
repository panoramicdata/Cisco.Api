using System.Runtime.Serialization;

namespace Cisco.Api.Data.SecurityAdvisories;

[DataContract]
public class OsVersion
{
	[DataMember(Name = "nos_version")]
	public string NosVersion { get; set; } = string.Empty;

	[DataMember(Name = "nos_type")]
	public string NosType { get; set; } = string.Empty;

	[DataMember(Name = "platform_name")]
	public string PlatformName { get; set; } = string.Empty;
}