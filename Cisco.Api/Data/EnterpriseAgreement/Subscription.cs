using System.Collections.Generic;
using System.Runtime.Serialization;
using System;

namespace Cisco.Api.Data.EnterpriseAgreement.Responses;

[DataContract]
public class Subscription
{
	[DataMember(Name = "subscriptionID")]
	public string SubscriptionID { get; set; } = string.Empty;

	[DataMember(Name = "status")]
	public string Status { get; set; } = string.Empty;

	[DataMember(Name = "startDate")]
	public DateTimeOffset StartDate { get; set; }

	[DataMember(Name = "endDate")]
	public DateTimeOffset EndDate { get; set; }

	[DataMember(Name = "duration")]
	public int Duration { get; set; }

	[DataMember(Name = "remainingDuration")]
	public int RemainingDuration { get; set; }

	[DataMember(Name = "durationInMonths")]
	public int DurationInMonths { get; set; }

	[DataMember(Name = "remainingDurationInMonths")]
	public int RemainingDurationInMonths { get; set; }

	[DataMember(Name = "nextTrueForward")]
	public DateTimeOffset NextTrueForward { get; set; }

	[DataMember(Name = "architectureName")]
	public string ArchitectureName { get; set; } = string.Empty;

	[DataMember(Name = "accounts")]
	public List<SubscriptionAccount> Accounts { get; set; } = [];
}
