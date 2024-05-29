using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public class ReportPayloadParentWithoutItems
{
	/// <summary>
	/// The status.
	/// </summary>
	[DataMember(Name = "metadata")]
	public ReportPayloadMetadata Metadata { get; set; } = null!;

	/// <summary>
	/// The report items.
	/// </summary>
	[DataMember(Name = "items")]
	public List<ReportPayloadItem> Items { get; set; } = null!;
}
