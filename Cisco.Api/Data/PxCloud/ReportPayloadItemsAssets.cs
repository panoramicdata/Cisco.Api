using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public class ReportPayloadItemsAssets : ReportPayloadItem
{
	/// https://developer.cisco.com/docs/px-cloud/asset-and-license-view/#assets
	/// Parameters role, lastScan, managedBy, criticalSecurityAdvisories, nodeld are not applicable for Data Center Compute(DCC).
	/// Parameter nodeld is not applicable for Campus Network(IBN).
	/// Parameters addressLine1, addressLine2, adddressLine3, licenseStatus, licenseLevel, profileName, hclStatus, ucsDomain, subscriptionID, subscriptionStartDate, subscriptionEndDate, hxCluster are not applicable for Campus Network(IBN) and Data Center Network(DCN).
	/// The values for contractNumber, coverageStartDate and coverageEndDate are masked for unauthorised user, and set as null.
	/// Parameters deviceSoftwareUpgradeChannel, orgId, orgName, belongsTo, devicePlatform, deviceDisplayStatus, webexDeviceId are exclusive to Collaboration Success Track.





	/// <summary>
	///	Unique identifier used in CX Cloud and PX Cloud to identify the asset.
	/// </summary>
	[DataMember(Name = "assetId")]
	public string AssetId { get; set; } = null!;

	/// <summary>
	/// Unique asset name.
	/// </summary>
	[DataMember(Name = "assetName")]
	public string AssetName { get; set; } = null!;

	/// <summary>
	/// Cisco product family of the asset, for example Cisco Catalyst 9300 Series Switches.
	/// </summary>
	[DataMember(Name = "productFamily")]
	public string ProductFamily { get; set; } = null!;

	/// <summary>
	/// Cisco product line classifier of the asset, for example Routers, Switches, and Wireless.
	/// </summary>
	[DataMember(Name = "productType")]
	public string ProductType { get; set; } = null!;

	/// <summary>
	/// Unique Cisco serial number of the asset used for product identification.
	/// </summary>
	[DataMember(Name = "serialNumber")]
	public string SerialNumber { get; set; } = null!;

	/// <summary>
	/// Cisco product number of the asset.
	/// </summary>
	[DataMember(Name = "productId")]
	public string ProductId { get; set; } = null!;

	/// <summary>
	/// IP address assigned to the asset.
	/// </summary>
	[DataMember(Name = "ipAddress")]
	public string IpAddress { get; set; } = null!;

	/// <summary>
	/// Cisco product description.
	/// </summary>
	[DataMember(Name = "productDescription")]
	public string ProductDescription { get; set; } = null!;

	/// <summary>
	/// Connected: Asset was collected from controllers integrated with CX Cloud. Not Connected: Asset was not collected from controllers integrated with CX Cloud.
	/// </summary>
	[DataMember(Name = "connectionStatus")]
	public string ConnectionStatus { get; set; } = null!;

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
	/// Location where the asset is deployed.
	/// </summary>
	[DataMember(Name = "location")]
	public string Location { get; set; } = null!;

	/// <summary>
	/// Success tracks associated with the asset.
	/// </summary>
	[DataMember(Name = "successTrack")]
	public List<ReportPayloadItemsSuccessTrack> SuccessTracks { get; set; } = null!;

	/// <summary>
	/// The date when the end of sale and the end of life milestones for a product, service or subscription is communicated to the general public.
	/// </summary>
	[DataMember(Name = "endOfLifeAnnounced")]
	public string? EndOfLifeAnnounced { get; set; } = null!;

	/// <summary>
	/// The date from which the product, service or subscription is no longer available for sale.
	/// </summary>
	[DataMember(Name = "endOfSale")]
	public string? EndOfSale { get; set; } = null!;

	/// <summary>
	/// Last possible ship date that can be requested from Cisco and/or its contract manufacturers.
	/// </summary>
	[DataMember(Name = "lastShip")]
	public string? LastShip { get; set; } = null!;

	/// <summary>
	/// Last date a routine failure analysis may be performed to determine the cause of hardware product failure or defect.
	/// </summary>
	[DataMember(Name = "endOfRoutineFailureAnalysis")]
	public string? EndOfRoutineFailureAnalysis { get; set; } = null!;

	/// <summary>
	/// Last date to add (attach) a new service contract (hardware, OS software and application software).
	/// </summary>
	[DataMember(Name = "endOfNewServiceAttach")]
	public string? EndOfNewServiceAttach { get; set; } = null!;

	/// <summary>
	/// Last date to renew service contracts (including managed service contracts) for hardware, operating system (OS) software
	/// and application software as long as the contract end date does not exceed Last Day Of Support(LDOS).
	/// </summary>
	[DataMember(Name = "endOfServiceContractRenewal")]
	public string? EndOfServiceContractRenewal { get; set; } = null!;

	/// <summary>
	/// The last date to receive applicable service and support as entitled by active service contracts for covered products.
	/// After this date, the service is no longer available.
	/// </summary>
	[DataMember(Name = "ldosDate")]
	public string? LdosDate { get; set; } = null!;

	/// <summary>
	/// Status of the support contract, which can be Covered, Not_Covered, or Covered_By_Other.
	/// Covered_By_Other indicates your partner organization is not associated with the support contract that covers the asset.
	/// </summary>
	[DataMember(Name = "coverageStatus")]
	public string CoverageStatus { get; set; } = null!;

	/// <summary>
	/// Contract ID of the support contract that covers the asset. This value will be null if the coverageStatus is "Not_Covered" or "Covered_By_Other".
	/// </summary>
	[DataMember(Name = "contractNumber")]
	public string? ContractNumber { get; set; }

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
	/// The type of support contract that covers the asset, for example CX Level 2, SNTC, and SSPT PLUS.
	/// </summary>
	[DataMember(Name = "supportType")]
	public string SupportType { get; set; } = null!;

	/// <summary>
	/// Number of advisories the asset is exposed to, which includes Security Advisories, Field Notices, and Priority Bugs.
	/// </summary>
	[DataMember(Name = "advisories")]
	public int Advisories { get; set; }

	/// <summary>
	/// Type of asset, for example Hardware, Software, and XaaS.
	/// </summary>
	[DataMember(Name = "assetType")]
	public string AssetType { get; set; } = null!;

	//////////////////////
	/// Exclusive to Collaboration Success Track.

	/// <summary>
	/// The upgrade channel the device is assigned to.
	/// </summary>
	[DataMember(Name = "deviceSoftwareUpgradeChannel")]
	public string DeviceSoftwareUpgradeChannel { get; set; } = null!;

	/// <summary>
	/// The ID of the organization associated with the device.
	/// </summary>
	[DataMember(Name = "orgId")]
	public string OrgId { get; set; } = null!;

	/// <summary>
	/// The name of the organization associated with the device.
	/// </summary>
	[DataMember(Name = "orgName")]
	public string OrgName { get; set; } = null!;

	/// <summary>
	/// The workspace or person associated with the device.
	/// </summary>
	[DataMember(Name = "belongsTo")]
	public string BelongsTo { get; set; } = null!;

	/// <summary>
	/// Indicates the native meeting platform for the device (Cisco or Microsoft).
	/// </summary>
	[DataMember(Name = "devicePlatform")]
	public string DevicePlatform { get; set; } = null!;

	/// <summary>
	/// Indicates the current status of the device like Online, Offline, Online with Issues, Expired, Activating, or Status Unavailable.
	/// </summary>
	[DataMember(Name = "deviceDisplayStatus")]
	public string DeviceDisplayStatus { get; set; } = null!;

	/// <summary>
	/// The Universally Unique Identifier (UUID).
	/// </summary>
	[DataMember(Name = "webexDeviceId")]
	public string WebexDeviceId { get; set; } = null!;

	//////////////////////
	/// Not applicable to Campus Network (IBN) or Data Center Compute(DCC).

	/// <summary>
	/// Unique node identifier assigned to the asset.
	/// </summary>
	[DataMember(Name = "nodeId")]
	public string NodeId { get; set; } = null!;

	//////////////////////
	/// Not applicable for Data Center Compute (DCC).

	/// <summary>
	/// Role assigned to the asset, which is used to identify assets according to their responsibilities and placement in the network.
	/// </summary>
	[DataMember(Name = "role")]
	public string Role { get; set; } = null!;

	/// <summary>
	/// Last successful scheduled or on-demand diagnostic analysis performed on the asset.
	/// </summary>
	[DataMember(Name = "lastScan")]
	public string? LastScan { get; set; } = null!;

	/// <summary>
	/// IP address or fully qualified domain name of Cisco network management system that manages the asset.
	/// </summary>
	[DataMember(Name = "managedBy")]
	public string ManagedBy { get; set; } = null!;

	/// <summary>
	/// Number of critical impact security Advisories the asset is exposed to.
	/// </summary>
	[DataMember(Name = "criticalSecurityAdvisories")]
	public int CriticalSecurityAdvisories { get; set; }

	//////////////////////
	/// Not applicable for Campus Network(IBN) and Data Center Network(DCN).

	/// <summary>
	/// addressLine1, addressLine2, and addressLine3 together return the installation address of the asset.
	/// </summary>
	[DataMember(Name = "addressLine1")]
	public string? AddressLine1 { get; set; } = null!;

	/// <summary>
	/// addressLine1, addressLine2, and addressLine3 together return the installation address of the asset.
	/// </summary>
	[DataMember(Name = "addressLine2")]
	public string? AddressLine2 { get; set; } = null!;

	/// <summary>
	/// addressLine1, addressLine2, and addressLine3 together return the installation address of the asset.
	/// </summary>
	[DataMember(Name = "addressLine3")]
	public string? AddressLine3 { get; set; } = null!;

	/// <summary>
	/// Status of the license associated with the asset.
	/// </summary>
	[DataMember(Name = "licenseStatus")]
	public string? LicenseStatus { get; set; } = null!;

	/// <summary>
	/// Level of the license associated with the asset.
	/// </summary>
	[DataMember(Name = "licenseLevel")]
	public string? LicenseLevel { get; set; } = null!;

	/// <summary>
	/// Name of the profile associated with the asset.
	/// </summary>
	[DataMember(Name = "profileName")]
	public string? ProfileName { get; set; } = null!;

	/// <summary>
	/// Compliance status with the Hardware Compatibility List (HCL):
	/// • Validated: The server model, firmware, operating system, adapters, and drivers of the asset have been validated and the configuration is found in the compliance database.
	/// • Not Listed: The combination of the server model, firmware, operating system, adapters, and drivers of the asset are not a tested and validated configuration, or are not found in the compliance database.
	/// • Incomplete: Missing or incomplete data for the compliance validation.
	/// </summary>
	[DataMember(Name = "hclStatus")]
	public string? HclStatus { get; set; } = null!;

	/// <summary>
	/// Name of the UCS Domain the asset belongs to.
	/// </summary>
	[DataMember(Name = "ucsDomain")]
	public string? UcsDomain { get; set; } = null!;

	/// <summary>
	/// Subscription ID associated with the asset.
	/// </summary>
	[DataMember(Name = "subscriptionID")]
	public string? SubscriptionID { get; set; } = null!;

	/// <summary>
	/// Date the subscription for the asset begins.
	/// </summary>
	[DataMember(Name = "subscriptionStartDate")]
	public string? SubscriptionStartDate { get; set; } = null!;

	/// <summary>
	/// Date the subscription for the asset ends.
	/// </summary>
	[DataMember(Name = "subscriptionEndDate")]
	public string? SubscriptionEndDate { get; set; } = null!;

	/// <summary>
	/// Name of the HyperFlex cluster the asset belongs to.
	/// </summary>
	[DataMember(Name = "hxCluster")]
	public string? HxCluster { get; set; } = null!;
}
