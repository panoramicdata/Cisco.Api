using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public class Customer
{
	/// <summary>
	/// Customer name in CX Cloud.
	/// </summary>
	[DataMember(Name = "customerName")]
	public required string CustomerName { get; set; }

	/// <summary>
	/// Unique identifier of the customer.
	/// </summary>
	[DataMember(Name = "customerId")]
	public required string CustomerId { get; set; }

	/// <summary>
	/// Customer global ultimate name. The customer identity name at the Enterprise level.
	/// </summary>
	[DataMember(Name = "customerGUName")]
	public required string CustomerGUName { get; set; }

	/// <summary>
	/// List of Success Tracks.
	/// </summary>
	[DataMember(Name = "successTracks")]
	public List<CustomerSuccessTrack> SuccessTracks { get; set; } = [];
}
