using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing;

[DataContract]
[DebuggerDisplay("{License}")]
public class LicenseSubscription
{
	[DataMember(Name = "license")]
	public required string License { get; set; }

	[DataMember(Name = "quantity")]
	public required int Quantity { get; set; }

	[DataMember(Name = "billingType")]
	public required string BillingType { get; set; }

	[DataMember(Name = "subscriptionId")]
	public required string SubscriptionId { get; set; }

	[DataMember(Name = "virtualAccount")]
	public required string VirtualAccount { get; set; }

	[DataMember(Name = "startDate")]
	public required DateTimeOffset StartDate { get; set; }

	[DataMember(Name = "endDate")]
	public required DateTimeOffset EndDate { get; set; }

	[DataMember(Name = "status")]
	public required string Status { get; set; }
}
