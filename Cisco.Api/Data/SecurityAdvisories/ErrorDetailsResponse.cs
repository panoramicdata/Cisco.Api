using System.Runtime.Serialization;

namespace Cisco.Api.Data.SecurityAdvisories;

/// <summary>
/// Represents the error details response.
/// </summary>
[DataContract]
public class ErrorDetailsResponse
{
	/// <summary>
	/// Gets or sets the error code.
	/// </summary>
	[DataMember(Name = "errorCode")]
	public string ErrorCode { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the error message.
	/// </summary>
	[DataMember(Name = "errorMessage")]
	public string ErrorMessage { get; set; } = string.Empty;
}