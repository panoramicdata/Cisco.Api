using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public class Customers : BaseResponse
{
	/// <summary>
	/// The list of customers.
	/// </summary>
	[DataMember(Name = "items")]
	public List<Customer> Items { get; set; } = null!;
}
