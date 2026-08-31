using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

/// <summary>
/// Represents the report payload parent field notices.
/// </summary>
[DataContract]
public class ReportPayloadParentFieldNotices : ReportPayloadParent
{
	/// <summary>
	/// The report items.
	/// </summary>
	[DataMember(Name = "items")]
	public List<ReportPayloadItemsFieldNotices> Items { get; set; } = null!;
}
