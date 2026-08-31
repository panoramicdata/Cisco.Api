using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

/// <summary>
/// Represents the report payload parent licenses with assets.
/// </summary>
[DataContract]
public class ReportPayloadParentLicensesWithAssets : ReportPayloadParent
{
	/// <summary>
	/// The report items.
	/// </summary>
	[DataMember(Name = "items")]
	public List<ReportPayloadItemsLicensesWithAssets> Items { get; set; } = null!;
}
