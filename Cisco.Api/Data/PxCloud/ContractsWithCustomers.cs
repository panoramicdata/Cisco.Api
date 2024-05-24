using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public class ContractsWithCustomers : BaseResponse
{
	/// <summary>
	/// The list of contracts with customers.
	/// </summary>
	[DataMember(Name = "items")]
	public List<ContractWithCustomers> Items { get; set; } = null!;
}
