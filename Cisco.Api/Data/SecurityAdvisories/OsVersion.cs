using System.Runtime.Serialization;

namespace Cisco.Api.Data.SecurityAdvisories;

/// <summary>
/// Represents the OS version.
/// </summary>
[DataContract]
public class OsVersion
{
	/// <summary>
	/// Gets or sets the nos version.
	/// </summary>
	[DataMember(Name = "nos_version")]
	public string NosVersion { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the nos type.
	/// </summary>
	[DataMember(Name = "nos_type")]
	public string NosType { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the platform name.
	/// </summary>
	[DataMember(Name = "platform_name")]
	public string PlatformName { get; set; } = string.Empty;
}