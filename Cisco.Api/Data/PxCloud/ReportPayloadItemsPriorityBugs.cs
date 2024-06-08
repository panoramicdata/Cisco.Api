using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public class ReportPayloadItemsPriorityBugs : ReportPayloadItem
{
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

	/// </summary>
	[DataMember(Name = "serialNumber")]
	public string? SerialNumber { get; set; }

	/// <summary>
	/// Cisco product number of the asset.
	/// </summary>
	[DataMember(Name = "productId")]
	public string? ProductId { get; set; }

	/// <summary>
	/// IP address assigned to the asset.
	/// </summary>
	[DataMember(Name = "ipAddress")]
	public string? IpAddress { get; set; }

	/// <summary>
	/// Release of the Cisco software running on the asset.
	/// </summary>
	[DataMember(Name = "softwareRelease")]
	public string? SoftwareRelease { get; set; }

	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "bugId")]
	public string? BugId { get; set; }

	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "bugTitle")]
	public string? BugTitle { get; set; }

	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "description")]
	public string? Description { get; set; }

	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "url")]
	public string? Url { get; set; }

	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "bugSeverity")]
	public string? BugSeverity { get; set; }

	/// <summary>
	/// Success tracks associated with the asset.
	/// </summary>
	[DataMember(Name = "successTrack")]
	public List<ReportPayloadItemsSuccessTrack> SuccessTracks { get; set; } = [];

	/// <summary>
	///
	/// </summary>
	[DataMember(Name = "impact")]
	public string? Impact { get; set; }
}
