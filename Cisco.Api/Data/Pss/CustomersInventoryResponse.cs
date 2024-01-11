using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

/// <summary>
/// CustomersInventory Response
/// </summary>
[XmlRoot("CustomersInventoryResponse", Namespace = "http://www.cisco.com/InventoryService")]
public class CustomersInventoryResponse
{
	/// <summary>
	/// The customer inventories
	/// </summary>
	[XmlElement("customerInventory")]
	public List<CustomerInventory> CustomerInventories { get; set; } = null!;

	/// <summary>
	/// The time stamp indicates when this service call was performed.
	/// </summary>
	[XmlElement("responseTimestamp")]
	public DateTime ResponseTimestamp { get; set; }

	[XmlElement("message")]
	public Message Message { get; set; } = null!;
}
