using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public class ReportPayloadItemsSecurityAdvisories // : ReportPayloadItem
{
	/// Not documented

	/// <summary>
	///	Unique identifier used in CX Cloud and PX Cloud to identify the asset.
	/// </summary>
	[DataMember(Name = "assetId")]
	public string? AssetId { get; set; }

	/// <summary>
	/// Unique asset name.
	/// </summary>
	[DataMember(Name = "assetName")]
	public string? AssetName { get; set; }

	/// <summary>
	/// IP address assigned to the asset.
	/// </summary>
	[DataMember(Name = "ipAddress")]
	public string? IpAddress { get; set; }

	/// <summary>
	/// Unique Cisco serial number of the asset used for product identification.
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
	[DataMember(Name = "advisoryId")]
	public string? AdvisoryId { get; set; }
	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "impact")]
	public string? Impact { get; set; }
	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "cvss")]
	public string? Cvss { get; set; }
	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "version")]
	public string? Version { get; set; }
	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "cve")]
	public string? Cve { get; set; }
	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "published")]
	public string? Published { get; set; }
	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "updated")]
	public string? Updated { get; set; }
	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "advisory")]
	public string? Advisory { get; set; }
	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "summary")]
	public string? Summary { get; set; }
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
	public List<string>? AffectedReason { get; set; }

	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "softwareRelease")]
	public string? SoftwareRelease { get; set; }
	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "productId")]
	public string? ProductId { get; set; }
}
