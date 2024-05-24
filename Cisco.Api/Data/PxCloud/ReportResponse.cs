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
	/// The suggested next poll time.
	/// </summary>
	[DataMember(Name = "suggestedNextPollTime")]
	public int SuggestedNextPollTime { get; set; }

	/// <summary>
	/// The estimated completion time.
	/// </summary>
	[DataMember(Name = "estimatedCompletionTime")]
	public string EstimatedCompletionTime { get; set; } = null!;
}
