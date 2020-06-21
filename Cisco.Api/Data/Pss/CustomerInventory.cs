using System.Diagnostics;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	[DebuggerDisplay("{Customer.Name}")]
	[XmlRoot("customerInventory", Namespace = "http://www.cisco.com/InventoryService")]
	public class CustomerInventory
	{
		[XmlElement("customer")]
		public Customer Customer { get; set; } = null!;

		[XmlElement("inventory")]
		public Inventory Inventory { get; set; } = null!;
	}
}