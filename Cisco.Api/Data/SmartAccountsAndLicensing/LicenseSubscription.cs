using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing;

/// <summary>
/// Represents the license subscription.
/// </summary>
[DataContract]
[DebuggerDisplay("{License}")]
public class LicenseSubscription
{
	/// <summary>
	/// Gets or sets the license.
	/// </summary>
	[DataMember(Name = "license")]
	public required string License { get; set; }

	/// <summary>
	/// Gets or sets the quantity.
	/// </summary>
	[DataMember(Name = "quantity")]
	public required int Quantity { get; set; }

	/// <summary>
	/// Gets or sets the billing type.
	/// </summary>
	[DataMember(Name = "billingType")]
	public required string BillingType { get; set; }

	/// <summary>
	/// Gets or sets the subscription ID.
	/// </summary>
	[DataMember(Name = "subscriptionId")]
	public required string SubscriptionId { get; set; }

	/// <summary>
	/// Gets or sets the virtual account.
	/// </summary>
	[DataMember(Name = "virtualAccount")]
	public required string VirtualAccount { get; set; }

	/// <summary>
	/// Gets or sets the start date.
	/// </summary>
	[DataMember(Name = "startDate")]
	public required DateTimeOffset StartDate { get; set; }

	/// <summary>
	/// Gets or sets the end date.
	/// </summary>
	[DataMember(Name = "endDate")]
	public required DateTimeOffset EndDate { get; set; }

	/// <summary>
	/// Gets or sets the status.
	/// </summary>
	[DataMember(Name = "status")]
	public required string Status { get; set; }
}
