using System.Diagnostics;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing;

/// <summary>
/// Represents the license detail.
/// </summary>
[DataContract]
[DebuggerDisplay("{Customer}")]
public class LicenseDetail
{
	/// <summary>
	/// Gets or sets the customer.
	/// </summary>
	[DataMember(Name = "customer")]
	public string? Customer { get; set; }

	/// <summary>
	/// Gets or sets the quantity.
	/// </summary>
	[DataMember(Name = "quantity")]
	public int Quantity { get; set; }

	/// <summary>
	/// Gets or sets the order number.
	/// </summary>
	[DataMember(Name = "orderNumber")]
	public string? OrderNumber { get; set; }

	/// <summary>
	/// Gets or sets the bill to po.
	/// </summary>
	[DataMember(Name = "billToPo")]
	public string? BillToPo { get; set; }

	/// <summary>
	/// Gets or sets the license type.
	/// </summary>
	[DataMember(Name = "licenseType")]
	public LicenseType LicenseType { get; set; }

	/// <summary>
	/// Gets or sets the license SKU.
	/// </summary>
	[DataMember(Name = "licenseSku")]
	public string LicenseSku { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the end customer po.
	/// </summary>
	[DataMember(Name = "endCustomerPo")]
	public string? EndCustomerPo { get; set; }

	/// <summary>
	/// Gets or sets the order line identifier.
	/// </summary>
	[DataMember(Name = "orderLineIdentifier")]
	public string? OrderLineIdentifier { get; set; }

	/// <summary>
	/// Gets or sets the subscription ID.
	/// </summary>
	[DataMember(Name = "subscriptionId")]
	public string? SubscriptionId { get; set; }

	/// <summary>
	/// Gets or sets the start date.
	/// </summary>
	[DataMember(Name = "startDate")]
	public string? StartDate { get; set; }

	/// <summary>
	/// Gets or sets the end date.
	/// </summary>
	[DataMember(Name = "endDate")]
	public string? EndDate { get; set; }

	/// <summary>
	/// Gets or sets the ship to.
	/// </summary>
	[DataMember(Name = "shipTo")]
	public string? ShipTo { get; set; }

	/// <summary>
	/// Gets or sets the status.
	/// </summary>
	[DataMember(Name = "status")]
	public LicenseStatus Status { get; set; }
}
