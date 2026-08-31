using System.Collections.Generic;
using System.Runtime.Serialization;
using System;

namespace Cisco.Api.Data.EnterpriseAgreement.Responses;

/// <summary>
/// Represents the subscription.
/// </summary>
[DataContract]
public class Subscription
{
	/// <summary>
	/// Gets or sets the subscription ID.
	/// </summary>
	[DataMember(Name = "subscriptionID")]
	public string SubscriptionID { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the status.
	/// </summary>
	[DataMember(Name = "status")]
	public string Status { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the start date.
	/// </summary>
	[DataMember(Name = "startDate")]
	public DateTimeOffset StartDate { get; set; }

	/// <summary>
	/// Gets or sets the end date.
	/// </summary>
	[DataMember(Name = "endDate")]
	public DateTimeOffset EndDate { get; set; }

	/// <summary>
	/// Gets or sets the duration.
	/// </summary>
	[DataMember(Name = "duration")]
	public int Duration { get; set; }

	/// <summary>
	/// Gets or sets the remaining duration.
	/// </summary>
	[DataMember(Name = "remainingDuration")]
	public int RemainingDuration { get; set; }

	/// <summary>
	/// Gets or sets the duration in months.
	/// </summary>
	[DataMember(Name = "durationInMonths")]
	public int DurationInMonths { get; set; }

	/// <summary>
	/// Gets or sets the remaining duration in months.
	/// </summary>
	[DataMember(Name = "remainingDurationInMonths")]
	public int RemainingDurationInMonths { get; set; }

	/// <summary>
	/// Gets or sets the next true forward.
	/// </summary>
	[DataMember(Name = "nextTrueForward")]
	public DateTimeOffset NextTrueForward { get; set; }

	/// <summary>
	/// Gets or sets the architecture name.
	/// </summary>
	[DataMember(Name = "architectureName")]
	public string ArchitectureName { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the accounts.
	/// </summary>
	[DataMember(Name = "accounts")]
	public List<SubscriptionAccount> Accounts { get; set; } = [];
}
