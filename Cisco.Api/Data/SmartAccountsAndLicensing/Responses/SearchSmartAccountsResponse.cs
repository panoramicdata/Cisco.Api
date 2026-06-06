using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing.Responses;

/// <summary>
/// Search smart accounts response
/// </summary>
[DataContract]
public class SearchSmartAccountsResponse : BaseResponse
{
	/// <summary>
	/// The smart accounts with IDs
	/// </summary>
	[DataMember(Name = "accounts")]
	public List<SmartAccountWithIds> Accounts { get; set; } = null!;

	/// <summary>
	/// Total records
	/// </summary>
	[DataMember(Name = "totalRecords")]
	public int TotalRecords { get; set; }
}