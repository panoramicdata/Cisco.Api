using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public class ReportPayloadItemsLicensesWithAssets // : ReportPayloadItem
{
	/// https://developer.cisco.com/docs/px-cloud/asset-and-license-view/#licenses-with-assets

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
	/// Product family of the license.
	/// </summary>
	[DataMember(Name = "productFamily")]
	public string? ProductFamily { get; set; }

	/// <summary>
	/// Cisco product number of the asset.
	/// </summary>
	[DataMember(Name = "productType")]
	public string? ProductType { get; set; }

	/// <summary>
	/// Connected: Asset was collected from controllers integrated with CX Cloud.
	/// Not Connected: Asset was not collected from controllers integrated with CX Cloud.
	/// </summary>
	[DataMember(Name = "connectionStatus")]
	public string? ConnectionStatus { get; set; }

	/// <summary>
	/// Cisco product description.
	/// </summary>
	[DataMember(Name = "productDescription")]
	public string? ProductDescription { get; set; }

	/// <summary>
	/// Success tracks associated with the asset.
	/// </summary>
	[DataMember(Name = "successTrack")]
	public List<ReportPayloadItemsSuccessTrack> SuccessTracks { get; set; } = [];

	/// <summary>
	///	Cisco product number of the license or license pool.
	/// </summary>
	[DataMember(Name = "license")]
	public string? License { get; set; }

	/// <summary>
	/// Start date of the license.
	/// </summary>
	[DataMember(Name = "licenseStartDate")]
	public string? LicenseStartDate { get; set; }

	/// <summary>
	/// End date of the license.
	/// </summary>
	[DataMember(Name = "licenseEndDate")]
	public string? LicenseEndDate { get; set; }

	/// <summary>
	/// Contract ID associated with the license.
	/// </summary>
	[DataMember(Name = "contractNumber")]
	public string? ContractNumber { get; set; }

	/// <summary>
	/// Subscription ID associated with the asset.
	/// </summary>
	[DataMember(Name = "subscriptionID")]
	public string? SubscriptionID { get; set; }

	/// <summary>
	/// The type of support contract that covers the asset, for example CX Level 2, SNTC, and SSPT PLUS.
	/// </summary>
	[DataMember(Name = "supportType")]
	public string? SupportType { get; set; }
}
