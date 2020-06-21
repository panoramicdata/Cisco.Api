using System.Diagnostics;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	[DebuggerDisplay("{Id}:{Name}")]
	[XmlRoot("customer", Namespace = "http://www.cisco.com/InventoryService")]
	public class Customer
	{
		[XmlElement("customerId")]
		public string Id { get; set; } = null!;

		[XmlElement("customerName")]
		public string Name { get; set; } = null!;
	}
}