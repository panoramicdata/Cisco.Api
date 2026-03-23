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
	/// A list of virtual accounts for which users intend to fetch the available licenses.
	/// Defaults to ["DEFAULT"]. While the Cisco API documentation states this is optional,
	/// omitting it via Refit causes request failures. Most Smart Accounts have a virtual
	/// account named "DEFAULT". Override this to target specific virtual accounts.
	/// </summary>
	[DataMember(Name = "virtualAccounts")]
	public List<string> VirtualAccounts { get; set; } = ["DEFAULT"];
}
