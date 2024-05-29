using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public class ReportResponse
{
	/// <summary>
	/// The status.
	/// </summary>
	[DataMember(Name = "status")]
	public string Status { get; set; } = null!;

	/// <summary>
	/// The suggested next poll time, in minutes.
	/// </summary>
	[DataMember(Name = "suggestedNextPollTimeInMins")]
	public int SuggestedNextPollTimeInMins { get; set; }

	/// <summary>
	/// The suggested next poll interval.
	/// </summary>
	[DataMember(Name = "suggestedNextPollInterval")]
	public int SuggestedNextPollInterval { get; set; }
}
