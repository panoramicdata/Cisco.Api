using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public class ReportPayloadItemsPurchasedLicenses // : ReportPayloadItem
{
	/// https://developer.cisco.com/docs/px-cloud/asset-and-license-view/#purchased-licenses

	/// <summary>
	///	Cisco product number of the license or license pool.
	/// </summary>
	[DataMember(Name = "license")]
	public string License { get; set; } = null!;

	/// <summary>
	/// Level of the license, for example DNA Advantage, ISE Plus and DCN Premier.
	/// </summary>
	[DataMember(Name = "licenseLevel")]
	public string LicenseLevel { get; set; } = null!;

	/// <summary>
	/// Number of licenses purchased on the contract for the license product number.
	/// </summary>
	[DataMember(Name = "purchasedQuantity")]
	public string PurchasedQuantity { get; set; } = null!;

	/// <summary>
	/// Product family of the license.
	/// </summary>
	[DataMember(Name = "productFamily")]
	public string ProductFamily { get; set; } = null!;

	/// <summary>
	/// Success tracks associated with the asset.
	/// </summary>
	[DataMember(Name = "successTrack")]
	public List<ReportPayloadItemsSuccessTrack> SuccessTracks { get; set; } = null!;

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
}
