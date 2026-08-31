using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

/// <summary>
/// Represents the report payload parent priority bugs.
/// </summary>
[DataContract]
public class ReportPayloadParentPriorityBugs : ReportPayloadParent
{
	/// <summary>
	/// The report items.
	/// </summary>
	[DataMember(Name = "items")]
	public List<ReportPayloadItemsPriorityBugs> Items { get; set; } = [];
}
