using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.EnterpriseAgreement.Responses;

/// <summary>
/// EA Smart Account All Subscription Consumption Report Response
/// </summary>
[DataContract]
public class EaSmartAccountAllSubscriptionConsumptionReportResponse
{
	/// <summary>
	/// Subscriptions
	/// </summary>
	[DataMember(Name = "subscriptions")]
	public List<Subscription> Subscriptions { get; set; } = [];
}