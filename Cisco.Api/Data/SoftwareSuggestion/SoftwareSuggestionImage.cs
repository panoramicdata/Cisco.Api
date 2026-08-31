using System.Runtime.Serialization;

namespace Cisco.Api.Data.SoftwareSuggestion;

/// <summary>
/// Represents the software suggestion image.
/// </summary>
[DataContract]
public class SoftwareSuggestionImage
{
	/// <summary>
	/// Gets or sets the image name.
	/// </summary>
	[DataMember(Name = "imageName")]
	public string ImageName { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the image size.
	/// </summary>
	[DataMember(Name = "imageSize")]
	public string ImageSize { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the feature set.
	/// </summary>
	[DataMember(Name = "featureSet")]
	public string FeatureSet { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the description.
	/// </summary>
	[DataMember(Name = "description")]
	public string Description { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the required dram.
	/// </summary>
	[DataMember(Name = "requiredDRAM")]
	public string RequiredDram { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the required flash.
	/// </summary>
	[DataMember(Name = "requiredFlash")]
	public string RequiredFlash { get; set; } = string.Empty;
}