using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing.Responses;

/// <summary>
/// List of license subscriptions response
/// </summary>
[DataContract]
public class ListOfLicenseSubscriptionsResponse : BaseResponse
{
	/// <summary>
	/// The license subscriptions
	/// </summary>
	[DataMember(Name = "licenseSubscriptions")]
	public List<LicenseSubscription> LicenseSubscriptions { get; set; } = null!;

	/// <summary>
	/// Total records
	/// </summary>
	[DataMember(Name = "totalRecords")]
	public int TotalRecords { get; set; }
}