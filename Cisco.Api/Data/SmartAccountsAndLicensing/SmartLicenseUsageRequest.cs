using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing;

/// <summary>
/// Request body for the POST /v1/accounts/{smartAccountDomain}/licenses endpoint.
/// </summary>
[DataContract]
public class SmartLicenseUsageRequest
{
	/// <summary>
	/// A list of virtual accounts for which users intend to fetch the available licenses.
	/// Defaults to ["DEFAULT"]. While the Cisco API documentation states this is optional,
	/// omitting it via Refit causes request failures. Most Smart Accounts have a virtual
	/// account named "DEFAULT". Override this to target specific virtual accounts.
	/// </summary>
	[DataMember(Name = "virtualAccounts")]
	public List<string> VirtualAccounts { get; set; } = ["DEFAULT"];
}
