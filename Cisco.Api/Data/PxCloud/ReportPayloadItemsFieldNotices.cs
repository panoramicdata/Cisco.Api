using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public class ReportPayloadItemsFieldNotices // : ReportPayloadItem
{
	/// Not documented
	///

	/// <summary>
	///	Unique identifier used in CX Cloud and PX Cloud to identify the asset.
	/// </summary>
	[DataMember(Name = "assetId")]
	public string? AssetId { get; set; }

	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "hwInstanceId")]
	public string? HwInstanceId { get; set; }

	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "assetName")]
	public string? AssetName { get; set; }

	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "productId")]
	public string? ProductId { get; set; }

	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "serialNumber")]
	public string? SerialNumber { get; set; }

	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "successTrack")]
	public List<ReportPayloadItemsSuccessTrack> SuccessTrack { get; set; } = [];

	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "fieldNoticeId")]
	public string? FieldNoticeId { get; set; }

	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "updated")]
	public string? Updated { get; set; }

	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "title")]
	public string? Title { get; set; }

	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "created")]
	public string? Created { get; set; }

	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "url")]
	public string? Url { get; set; }

	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "additionalNotes")]
	public string? AdditionalNotes { get; set; }

	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "affectedStatus")]
	public string? AffectedStatus { get; set; }

	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "affectedReason")]
	public List<string> AffectedReason { get; set; } = [];

	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "fieldNoticeDescription")]
	public string? FieldNoticeDescription { get; set; }

	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "softwareRelease")]
	public string? SoftwareRelease { get; set; }

	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "ipAddress")]
	public string? IpAddress { get; set; }
}
