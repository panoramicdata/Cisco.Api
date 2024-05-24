using System;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public class Contract
{
	/// <summary>
	/// Contract ID of the service contract.
	/// </summary>
	[DataMember(Name = "contractNumber")]
	public required string ContractNumber { get; set; }

	/// <summary>
	/// Internal identifier for a customer cuid will be null if onboardedstatus is 0.
	/// </summary>
	[DataMember(Name = "cuid")]
	public int? Cuid { get; set; }

	/// <summary>
	/// Identifier for a customer account view.
	/// </summary>
	[DataMember(Name = "cavid")]
	public int Cavid { get; set; }

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
	/// Name of the end customer. customerName will be null if onboardedstatus is 0.
	/// </summary>
	[DataMember(Name = "customerName")]
	public string? CustomerName { get; set; }

	/// <summary>
	/// Customer global ultimate name. The customer identity name at the Enterprise level.
	/// </summary>
	[DataMember(Name = "customerGUName")]
	public required string CustomerGUName { get; set; }

	/// <summary>
	/// Currency code for the country specified on the contract.
	/// </summary>
	[DataMember(Name = "currency")]
	public required string Currency { get; set; }

	/// <summary>
	/// Comma separated list of service levels of the contract.
	/// </summary>
	[DataMember(Name = "serviceLevel")]
	public required string ServiceLevel { get; set; }

	/// <summary>
	/// Comma separated list of allocated service group code of the contract.
	/// </summary>
	[DataMember(Name = "serviceProgram")]
	public required string ServiceProgram { get; set; }

	/// <summary>
	/// Start date of the contract, specified using the UTC date format YYYY-MM-DD.
	/// </summary>
	[DataMember(Name = "startDate")]
	public required String StartDate { get; set; }

	/// <summary>
	/// End date of the contract, specified using the UTC date format YYYY-MM-DD.
	/// </summary>
	[DataMember(Name = "endDate")]
	public required String EndDate { get; set; }

	/// <summary>
	/// Currency symbol.
	/// </summary>
	[DataMember(Name = "currencySymbol")]
	public required string CurrencySymbol { get; set; }

	/// <summary>
	/// Onboarding status of a customer in CX Cloud.
	/// </summary>
	[DataMember(Name = "onboardedstatus")]
	public int Onboardedstatus { get; set; }
}
