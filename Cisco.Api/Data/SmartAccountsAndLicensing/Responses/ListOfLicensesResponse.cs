using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing.Responses;

/// <summary>
/// List of licenses response
/// </summary>
[DataContract]
public class ListOfLicensesResponse : BaseResponse
{
	/// <summary>
	/// The licenses
	/// </summary>
	[DataMember(Name = "licenses")]
	public List<LicenseItem> Licenses { get; set; } = null!;

	/// <summary>
	/// Total records
	/// </summary>
	[DataMember(Name = "totalRecords")]
	public int TotalRecords { get; set; }
}