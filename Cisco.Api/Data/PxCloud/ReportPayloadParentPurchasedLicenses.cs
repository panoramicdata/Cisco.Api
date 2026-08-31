using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

/// <summary>
/// Represents the report payload parent purchased licenses.
/// </summary>
[DataContract]
public class ReportPayloadParentPurchasedLicenses : ReportPayloadParent
{
	/// <summary>
	/// The report items.
	/// </summary>
	[DataMember(Name = "items")]
	public List<ReportPayloadItemsPurchasedLicenses> Items { get; set; } = null!;
}
