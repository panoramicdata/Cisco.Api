using System;
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
	public required string ContractNumber { get; set; }

	/// <summary>
	/// Current contract status.
	/// </summary>
	[DataMember(Name = "contractStatus")]
	public required string ContractStatus { get; set; }

	/// <summary>
	/// Total monetary value of the contract.
	/// </summary>
	[DataMember(Name = "contractValue")]
	public int ContractValue { get; set; }

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
	public required string ServiceLevel { get; set; }

	/// <summary>
	/// Start date of the contract, specified using the UTC date format YYYY-MM-DD.
	/// </summary>
	[DataMember(Name = "coverageStartDate")]
	public required String CoverageStartDate { get; set; }

	/// <summary>
	/// End date of the contract, specified using the UTC date format YYYY-MM-DD.
	/// </summary>
	[DataMember(Name = "coverageEndDate")]
	public required String CoverageEndDate { get; set; }

	/// <summary>
	/// Onboarding status of a customer in CX Cloud.
	/// </summary>
	[DataMember(Name = "onboardedstatus")]
	public int Onboardedstatus { get; set; }
}
