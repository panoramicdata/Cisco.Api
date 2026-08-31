using System.Runtime.Serialization;

namespace Cisco.Api.Data.SoftwareSuggestion;

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
	/// Gets or sets the error description.
	/// </summary>
	[DataMember(Name = "errorDescription")]
	public string ErrorDescription { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the suggested action.
	/// </summary>
	[DataMember(Name = "suggestedAction")]
	public string SuggestedAction { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the input identifier.
	/// </summary>
	[DataMember(Name = "inputIdentifier")]
	public string InputIdentifier { get; set; } = string.Empty;
}