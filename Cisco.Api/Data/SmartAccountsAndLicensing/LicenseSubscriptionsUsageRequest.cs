using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing;

/// <summary>
/// Request body for the POST /v1/accounts/{smartAccountDomain}/license-subscriptions endpoint.
/// </summary>
[DataContract]
public class LicenseSubscriptionsUsageRequest
{
	/// <summary>
	/// The status of the subscriptions to be fetched. Valid values are 'Active', 'Canceled', 'Expired'.
	/// </summary>
	[DataMember(Name = "status")]
	public List<LicenseStatus> Status { get; set; } = [];

	/// <summary>
	/// An optional list of virtual accounts for which users intend to fetch the available licenses.
	/// If not specified, all the licenses from the domain for which the user has access to will be returned.
	/// </summary>
	[DataMember(Name = "virtualAccounts")]
	public List<string>? VirtualAccounts { get; set; }
}
