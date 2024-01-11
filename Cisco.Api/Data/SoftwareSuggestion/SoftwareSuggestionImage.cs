using System.Runtime.Serialization;

namespace Cisco.Api.Data.SoftwareSuggestion;

[DataContract]
public class SoftwareSuggestionImage
{
	[DataMember(Name = "imageName")]
	public string ImageName { get; set; } = string.Empty;

	[DataMember(Name = "imageSize")]
	public string ImageSize { get; set; } = string.Empty;

	[DataMember(Name = "featureSet")]
	public string FeatureSet { get; set; } = string.Empty;

	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	[DataMember(Name = "requiredDRAM")]
	public string RequiredDram { get; set; } = string.Empty;

	[DataMember(Name = "requiredFlash")]
	public string RequiredFlash { get; set; } = string.Empty;
}