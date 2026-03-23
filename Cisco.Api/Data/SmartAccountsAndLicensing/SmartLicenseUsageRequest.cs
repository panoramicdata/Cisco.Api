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
	/// An optional list of virtual accounts for which users intend to fetch the available licenses.
	/// If not specified, all the licenses from the domain for which the user has access to will be returned.
	/// </summary>
	[DataMember(Name = "virtualAccounts")]
	public List<string>? VirtualAccounts { get; set; }
}
