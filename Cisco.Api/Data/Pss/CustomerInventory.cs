using System.Diagnostics;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
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
		public Inventory Inventory { get; set; } = null!;
	}
}