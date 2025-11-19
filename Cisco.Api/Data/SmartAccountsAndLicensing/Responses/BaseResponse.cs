using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing.Responses;

[DataContract]
public class BaseResponse
{
	/// <summary>
	/// The status message
	/// </summary>
	[DataMember(Name = "statusMessage")]
	public string StatusMessage { get; set; } = string.Empty;

	/// <summary>
	/// The status
	/// </summary>
	[DataMember(Name = "status")]
	public string Status { get; set; } = string.Empty;
}
