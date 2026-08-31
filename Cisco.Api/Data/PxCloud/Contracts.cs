using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

/// <summary>
/// Represents the contracts.
/// </summary>
[DataContract]
public class Contracts : BaseResponse
{
	/// <summary>
	/// The list of contracts.
	/// </summary>
	[DataMember(Name = "items")]
	public List<Contract> Items { get; set; } = null!;
}
