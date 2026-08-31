using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

/// <summary>
/// Represents the report payload parent security advisories.
/// </summary>
[DataContract]
public class ReportPayloadParentSecurityAdvisories : ReportPayloadParent
{
	/// <summary>
	/// The report items.
	/// </summary>
	[DataMember(Name = "items")]
	public List<ReportPayloadItemsSecurityAdvisories> Items { get; set; } = null!;
}
