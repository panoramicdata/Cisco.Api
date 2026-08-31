using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

/// <summary>
/// Represents the contract details.
/// </summary>
[DataContract]
public class ContractDetails : BaseResponse
{
	/// <summary>
	/// The list of contract details.
	/// </summary>
	[DataMember(Name = "items")]
	public List<ContractDetail> Items { get; set; } = null!;
}
