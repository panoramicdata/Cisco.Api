using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SoftwareSuggestion;

/// <summary>
/// Represents the software suggestion.
/// </summary>
[DataContract]
public class SoftwareSuggestion
{
	/// <summary>
	/// Gets or sets the ID.
	/// </summary>
	[DataMember(Name = "id")]
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the is suggested.
	/// </summary>
	[DataMember(Name = "isSuggested")]
	public string IsSuggested { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the release format1.
	/// </summary>
	[DataMember(Name = "releaseFormat1")]
	public string ReleaseFormat1 { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the release format2.
	/// </summary>
	[DataMember(Name = "releaseFormat2")]
	public string ReleaseFormat2 { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the release date.
	/// </summary>
	[DataMember(Name = "releaseDate")]
	public string ReleaseDate { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the major release.
	/// </summary>
	[DataMember(Name = "majorRelease")]
	public string MajorRelease { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the release train.
	/// </summary>
	[DataMember(Name = "releaseTrain")]
	public string ReleaseTrain { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the release life cycle.
	/// </summary>
	[DataMember(Name = "releaseLifeCycle")]
	public string ReleaseLifeCycle { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the rel disp name.
	/// </summary>
	[DataMember(Name = "relDispName")]
	public string RelDispName { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the train disp name.
	/// </summary>
	[DataMember(Name = "trainDispName")]
	public string TrainDispName { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the images.
	/// </summary>
	[DataMember(Name = "images")]
	public List<SoftwareSuggestionImage> Images { get; set; } = [];

	/// <summary>
	/// Gets or sets the error details.
	/// </summary>
	[DataMember(Name = "errorDetailsResponse")]
	public ErrorDetailsResponse? ErrorDetails { get; set; }
}