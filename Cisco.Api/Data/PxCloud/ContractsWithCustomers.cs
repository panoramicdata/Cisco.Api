using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

/// <summary>
/// Represents the contracts with customers.
/// </summary>
[DataContract]
public class ContractsWithCustomers : BaseResponse
{
	/// <summary>
	/// The list of contracts with customers.
	/// </summary>
	[DataMember(Name = "items")]
	public List<ContractWithCustomers> Items { get; set; } = null!;
}
