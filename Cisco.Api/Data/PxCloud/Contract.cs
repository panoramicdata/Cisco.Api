using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public class Contract
{
	/// <summary>
	/// Contract ID of the service contract.
	/// </summary>
	[DataMember(Name = "contractNumber")]
	public string ContractNumber { get; set; } = null!;

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
	public string ContractStatus { get; set; } = null!;

	/// <summary>
	/// Total monetary value of the contract.
	/// </summary>
	[DataMember(Name = "contractValue")]
	public decimal ContractValue { get; set; }

	/// <summary>
	/// Name of the end customer. customerName will be null if onboardedstatus is 0.
	/// </summary>
	[DataMember(Name = "customerName")]
	public string? CustomerName { get; set; }

	/// <summary>
	/// Customer global ultimate name. The customer identity name at the Enterprise level.
	/// </summary>
	[DataMember(Name = "customerGUName")]
	public string CustomerGUName { get; set; } = null!;

	/// <summary>
	/// Currency code for the country specified on the contract.
	/// </summary>
	[DataMember(Name = "currency")]
	public string Currency { get; set; } = null!;

	/// <summary>
	/// Comma separated list of service levels of the contract.
	/// </summary>
	[DataMember(Name = "serviceLevel")]
	public string ServiceLevel { get; set; } = null!;

	/// <summary>
	/// Comma separated list of allocated service group code of the contract.
	/// </summary>
	[DataMember(Name = "serviceProgram")]
	public string ServiceProgram { get; set; } = null!;

	/// <summary>
	/// Start date of the contract, specified using the UTC date format YYYY-MM-DD.
	/// </summary>
	[DataMember(Name = "startDate")]
	public string StartDate { get; set; } = null!;

	/// <summary>
	/// End date of the contract, specified using the UTC date format YYYY-MM-DD.
	/// </summary>
	[DataMember(Name = "endDate")]
	public string EndDate { get; set; } = null!;

	/// <summary>
	/// Currency symbol.
	/// </summary>
	[DataMember(Name = "currencySymbol")]
	public string CurrencySymbol { get; set; } = null!;

	/// <summary>
	/// Onboarding status of a customer in CX Cloud.
	/// </summary>
	[DataMember(Name = "onboardedstatus")]
	public int Onboardedstatus { get; set; }
}
