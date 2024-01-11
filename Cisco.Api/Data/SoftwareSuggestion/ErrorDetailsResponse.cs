using System.Runtime.Serialization;

namespace Cisco.Api.Data.SoftwareSuggestion;

[DataContract]
public class ErrorDetailsResponse
{
	[DataMember(Name = "errorCode")]
	public string ErrorCode { get; set; } = string.Empty;

	[DataMember(Name = "errorDescription")]
	public string ErrorDescription { get; set; } = string.Empty;

	[DataMember(Name = "suggestedAction")]
	public string SuggestedAction { get; set; } = string.Empty;

	[DataMember(Name = "inputIdentifier")]
	public string InputIdentifier { get; set; } = string.Empty;
}