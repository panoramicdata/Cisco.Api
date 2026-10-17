using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

/// <summary>
/// Represents the report payload items hardware.
/// </summary>
[DataContract]
public class ReportPayloadItemsHardware : ReportPayloadItemsAssets
{
	/// https://developer.cisco.com/docs/px-cloud/asset-and-license-view/#hardware

	/// <summary>
	///	Unique identifier used in CX Cloud and PX Cloud to identify the hardware.
	/// </summary>
	[DataMember(Name = "hwInstanceId")]
	public string? HwInstanceId { get; set; }
}
