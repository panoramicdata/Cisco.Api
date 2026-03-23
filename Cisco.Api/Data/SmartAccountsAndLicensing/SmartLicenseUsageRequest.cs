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
	/// Defaults to ["DEFAULT"]. The Cisco API returns 422 if this field is omitted,
	/// despite documentation stating it is optional.
	/// </summary>
	[DataMember(Name = "virtualAccounts")]
	public List<string> VirtualAccounts { get; set; } = ["DEFAULT"];
}
