using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SoftwareSuggestion;

[DataContract]
public class SoftwareSuggestion
{
	[DataMember(Name = "id")]
	public int Id { get; set; }

	[DataMember(Name = "isSuggested")]
	public string IsSuggested { get; set; } = string.Empty;

	[DataMember(Name = "releaseFormat1")]
	public string ReleaseFormat1 { get; set; } = string.Empty;

	[DataMember(Name = "releaseFormat2")]
	public string ReleaseFormat2 { get; set; } = string.Empty;

	[DataMember(Name = "releaseDate")]
	public string ReleaseDate { get; set; } = string.Empty;

	[DataMember(Name = "majorRelease")]
	public string MajorRelease { get; set; } = string.Empty;

	[DataMember(Name = "releaseTrain")]
	public string ReleaseTrain { get; set; } = string.Empty;

	[DataMember(Name = "releaseLifeCycle")]
	public string ReleaseLifeCycle { get; set; } = string.Empty;

	[DataMember(Name = "relDispName")]
	public string RelDispName { get; set; } = string.Empty;

	[DataMember(Name = "trainDispName")]
	public string TrainDispName { get; set; } = string.Empty;

	[DataMember(Name = "images")]
	public List<SoftwareSuggestionImage> Images { get; set; } = [];

	[DataMember(Name = "errorDetailsResponse")]
	public ErrorDetailsResponse? ErrorDetails { get; set; }
}