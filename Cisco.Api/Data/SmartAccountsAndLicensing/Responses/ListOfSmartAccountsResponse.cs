using Cisco.Api.Data.SmartAccountsAndLicensing.Responses;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing;

/// <summary>
/// List of smart accounts response
/// </summary>
[DataContract]
public class ListOfSmartAccountsResponse : BaseResponse
{
	/// <summary>
	/// The smart accounts with roles
	/// </summary>
	[DataMember(Name = "accounts")]
	public List<SmartAccountWithRoles> Accounts { get; set; } = null!;
}