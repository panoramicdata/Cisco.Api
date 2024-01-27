using System.Runtime.Serialization;

namespace Cisco.Api.Data.SecurityAdvisories;

[DataContract]
public class ErrorDetailsResponse
{
	[DataMember(Name = "errorCode")]
	public string ErrorCode { get; set; } = string.Empty;

	[DataMember(Name = "errorMessage")]
	public string ErrorMessage { get; set; } = string.Empty;
}