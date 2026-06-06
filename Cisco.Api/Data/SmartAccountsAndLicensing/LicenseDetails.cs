using System.Diagnostics;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing;

[DataContract]
[DebuggerDisplay("{Customer}")]
public class LicenseDetail
{
	[DataMember(Name = "customer")]
	public string? Customer { get; set; }

	[DataMember(Name = "quantity")]
	public int Quantity { get; set; }

	[DataMember(Name = "orderNumber")]
	public string? OrderNumber { get; set; }

	[DataMember(Name = "billToPo")]
	public string? BillToPo { get; set; }

	[DataMember(Name = "licenseType")]
	public LicenseType LicenseType { get; set; }

	[DataMember(Name = "licenseSku")]
	public string LicenseSku { get; set; } = string.Empty;

	[DataMember(Name = "endCustomerPo")]
	public string? EndCustomerPo { get; set; }

	[DataMember(Name = "orderLineIdentifier")]
	public string? OrderLineIdentifier { get; set; }

	[DataMember(Name = "subscriptionId")]
	public string? SubscriptionId { get; set; }

	[DataMember(Name = "startDate")]
	public string? StartDate { get; set; }

	[DataMember(Name = "endDate")]
	public string? EndDate { get; set; }

	[DataMember(Name = "shipTo")]
	public string? ShipTo { get; set; }

	[DataMember(Name = "status")]
	public LicenseStatus Status { get; set; }
}
