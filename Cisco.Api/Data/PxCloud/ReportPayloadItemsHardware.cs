using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public class ReportPayloadItemsHardware // : ReportPayloadItem
{
	/// https://developer.cisco.com/docs/px-cloud/asset-and-license-view/#hardware

	/// <summary>
	///	Unique identifier used in CX Cloud and PX Cloud to identify the asset.
	/// </summary>
	[DataMember(Name = "assetId")]
	public string? AssetId { get; set; }

	/// <summary>
	///	Unique identifier used in CX Cloud and PX Cloud to identify the hardware.
	/// </summary>
	[DataMember(Name = "hwInstanceId")]
	public string? HwInstanceId { get; set; }

	/// <summary>
	/// Unique asset name.
	/// </summary>
	[DataMember(Name = "assetName")]
	public string? AssetName { get; set; }

	/// <summary>
	/// Cisco product family of the asset for example: Cisco Catalyst 9300 Series Switches.
	/// </summary>
	[DataMember(Name = "productFamily")]
	public string? ProductFamily { get; set; }

	/// <summary>
	/// Cisco product line classifier of the asset, for example Routers, Switches, and Wireless.
	/// </summary>
	[DataMember(Name = "productType")]
	public string? ProductType { get; set; }

	/// <summary>
	/// Cisco product number of the asset, for example, C9800-CL-K9 and WS-C4500X-16.
	/// </summary>
	[DataMember(Name = "productId")]
	public string? ProductId { get; set; }

	/// <summary>
	/// Unique Cisco serial number of the asset used for product identification.
	/// </summary>
	[DataMember(Name = "serialNumber")]
	public string? SerialNumber { get; set; }

	/// <summary>
	/// Success tracks associated with the asset.
	/// </summary>
	[DataMember(Name = "successTrack")]
	public List<ReportPayloadItemsSuccessTrack> SuccessTracks { get; set; } = [];

	/// <summary>
	/// The date when the document announces the end of sale and the end of life milestones for a product, service or subscription is communicated to the general public. UTC date format YYYY-MM-DD.
	/// </summary>
	[DataMember(Name = "endOfLifeAnnounced")]
	public string? EndOfLifeAnnounced { get; set; }

	/// <summary>
	/// The last date from which the product, service or subscription is no longer available for sale.
	/// </summary>
	[DataMember(Name = "endOfSale")]
	public string? EndOfSale { get; set; }

	/// <summary>
	/// The last ship date that can be requested from Cisco or its contract manufacturers. The actual ship date depends on lead time.
	/// </summary>
	[DataMember(Name = "lastShip")]
	public string? LastShip { get; set; }

	/// <summary>
	/// The last date a routine failure analysis may be performed to determine the cause of hardware product failure or defect.
	/// </summary>
	[DataMember(Name = "endOfRoutineFailureAnalysis")]
	public string? EndOfRoutineFailureAnalysis { get; set; }

	/// <summary>
	/// Last date to add (attach) a new service contract (hardware, OS software and application software).
	/// </summary>
	[DataMember(Name = "endOfNewServiceAttach")]
	public string? EndOfNewServiceAttach { get; set; }

	/// <summary>
	/// Last date to renew service contracts (including managed service contracts) for hardware, operating system (OS) software
	/// and application software as long as the contract end date does not exceed Last Day of Support (LDOS).
	/// </summary>
	[DataMember(Name = "endOfServiceContractRenewal")]
	public string? EndOfServiceContractRenewal { get; set; }

	/// <summary>
	/// The last date to receive applicable service and support as entitled by active service contracts for covered products.
	/// After this date, the service is no longer available.
	/// </summary>
	[DataMember(Name = "ldosDate")]
	public string? LdosDate { get; set; }

	/// <summary>
	/// End date of the support contract associated with the asset. This value will be null if the coverageStatus is "Not_Covered" or "Covered_By_Other".
	/// </summary>
	[DataMember(Name = "coverageEndDate")]
	public string? CoverageEndDate { get; set; }

	/// <summary>
	/// Start date of the support contract associated with the asset. This value will be null if the coverageStatus is "Not_Covered" or "Covered_By_Other".
	/// </summary>
	[DataMember(Name = "coverageStartDate")]
	public string? CoverageStartDate { get; set; }

	/// <summary>
	/// Contract ID of the support contract that covers the asset. This value will be null if the coverageStatus is "Not_Covered" or "Covered_By_Other".
	/// </summary>
	[DataMember(Name = "contractNumber")]
	public string? ContractNumber { get; set; }

	/// <summary>
	/// Status of the support contract, which can be Covered, Not_Covered, or Covered_By_Other. Covered_By_Other indicates your
	/// partner organization is not associated with the support contract that covers the asset.
	/// </summary>
	[DataMember(Name = "coverageStatus")]
	public string? CoverageStatus { get; set; }
}
