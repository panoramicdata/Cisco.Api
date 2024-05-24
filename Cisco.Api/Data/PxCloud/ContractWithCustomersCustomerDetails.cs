using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public class ContractWithCustomersCustomerDetails
{
	/// <summary>
	/// Customer name in CX Cloud.
	/// </summary>
	[DataMember(Name = "customerName")]
	public required string CustomerName { get; set; }

	/// <summary>
	/// Customer identifier in CX Cloud.
	/// </summary>
	[DataMember(Name = "customerId")]
	public required string CustomerId { get; set; }

	/// <summary>
	/// Customer global ultimate name. The customer identity name at the Enterprise level.
	/// </summary>
	[DataMember(Name = "customerGUName")]
	public required string CustomerGUName { get; set; }
}
