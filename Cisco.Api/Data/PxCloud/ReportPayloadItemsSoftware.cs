using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public class ReportPayloadItemsSoftware : ReportPayloadItem
{
	/// https://developer.cisco.com/docs/px-cloud/asset-and-license-view/#software

	/// <summary>
	/// Unique asset name.
	/// </summary>
	[DataMember(Name = "assetName")]
	public string AssetName { get; set; } = null!;

	/// <summary>
	///	Unique identifier used in CX Cloud and PX Cloud to identify the asset.
	/// </summary>
	[DataMember(Name = "assetId")]
	public string AssetId { get; set; } = null!;

	/// <summary>
	/// Cisco product number of the asset.
	/// </summary>
	[DataMember(Name = "productId")]
	public string ProductId { get; set; } = null!;

	/// <summary>
	/// Cisco software type running on the asset.
	/// </summary>
	[DataMember(Name = "softwareType")]
	public string SoftwareType { get; set; } = null!;

	/// <summary>
	/// Release of the Cisco software running on the asset.
	/// </summary>
	[DataMember(Name = "softwareRelease")]
	public string SoftwareRelease { get; set; } = null!;

	/// <summary>
	/// Success tracks associated with the asset.
	/// </summary>
	[DataMember(Name = "successTrack")]
	public List<ReportPayloadItemsSuccessTrack> SuccessTracks { get; set; } = null!;

	/// <summary>
	/// The date when the end of sale and the end of life milestones for a product, service or subscription is communicated to the general public.
	/// </summary>
	[DataMember(Name = "endOfLifeAnnounced")]
	public string EndOfLifeAnnounced { get; set; } = null!;

	/// <summary>
	/// Last date Cisco will release any final software maintenance releases, bug fixes, or test the product software.
	/// </summary>
	[DataMember(Name = "endOfSoftwareMaintenance")]
	public string EndOfSoftwareMaintenance { get; set; } = null!;

	/// <summary>
	/// The date from which the product, service or subscription is no longer available for sale.
	/// </summary>
	[DataMember(Name = "endOfSale")]
	public string EndOfSale { get; set; } = null!;

	/// <summary>
	/// The last date that Cisco may provide security fixes for vulnerabilities.
	/// </summary>
	[DataMember(Name = "endOfVulnerabilitySecuritySupport")]
	public string EndOfVulnerabilitySecuritySupport { get; set; } = null!;

	/// <summary>
	/// Last possible ship date that can be requested of Cisco or its contract manufacturers.
	/// </summary>
	[DataMember(Name = "lastShip")]
	public string LastShip { get; set; } = null!;

	/// <summary>
	/// Last date to receive applicable service and support as entitled by active service contracts for covered products.
	/// After this date, the service is no longer available.
	/// </summary>
	[DataMember(Name = "ldosDate")]
	public string LdosDate { get; set; } = null!;
}
