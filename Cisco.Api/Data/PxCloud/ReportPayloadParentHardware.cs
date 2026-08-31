using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

/// <summary>
/// Represents the report payload parent hardware.
/// </summary>
[DataContract]
public class ReportPayloadParentHardware : ReportPayloadParent
{
	/// <summary>
	/// The report items.
	/// </summary>
	[DataMember(Name = "items")]
	public List<ReportPayloadItemsHardware> Items { get; set; } = null!;
}
