using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public class ContractWithCustomers
{
	/// <summary>
	/// Contract ID of the service contract.
	/// </summary>
	[DataMember(Name = "contractNumber")]
	public string ContractNumber { get; set; } = null!;

	/// <summary>
	/// Current contract status.
	/// </summary>
	[DataMember(Name = "contractStatus")]
	public string ContractStatus { get; set; } = null!;

	/// <summary>
	/// Total monetary value of the contract.
	/// </summary>
	[DataMember(Name = "contractValue")]
	public decimal ContractValue { get; set; }

	/// <summary>
	/// List of Customer Details.
	/// </summary>
	[DataMember(Name = "customerDetails")]
	public List<ContractWithCustomersCustomerDetails> CustomerDetails { get; set; } = [];

	/// <summary>
	/// List of unique identifier for the Success Tracks.
	/// </summary>
	[DataMember(Name = "serviceTrackIds")]
	public List<int> ServiceTrackIds { get; set; } = [];

	/// <summary>
	/// Comma separated list of service levels of the contract.
	/// </summary>
	[DataMember(Name = "serviceLevel")]
	public string ServiceLevel { get; set; } = null!;

	/// <summary>
	/// Start date of the contract, specified using the UTC date format YYYY-MM-DD.
	/// </summary>
	[DataMember(Name = "coverageStartDate")]
	public string CoverageStartDate { get; set; } = null!;

	/// <summary>
	/// End date of the contract, specified using the UTC date format YYYY-MM-DD.
	/// </summary>
	[DataMember(Name = "coverageEndDate")]
	public string CoverageEndDate { get; set; } = null!;

	/// <summary>
	/// Onboarding status of a customer in CX Cloud.
	/// </summary>
	[DataMember(Name = "onboardedstatus")]
	public int Onboardedstatus { get; set; }
}
