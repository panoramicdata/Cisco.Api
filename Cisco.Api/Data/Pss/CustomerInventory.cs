using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

/// <summary>
/// The Customer Inventory
/// </summary>
[DebuggerDisplay("{Customer.Name}")]
[XmlRoot("customerInventory", Namespace = "http://www.cisco.com/InventoryService")]
public class CustomerInventory
{
	/// <summary>
	/// The customer
	/// </summary>
	[XmlElement("customer")]
	public Customer Customer { get; set; } = null!;

	/// <summary>
	/// The inventory
	/// </summary>
	[XmlElement("inventory")]
	public List<Inventory> Inventory { get; set; } = null!;
}