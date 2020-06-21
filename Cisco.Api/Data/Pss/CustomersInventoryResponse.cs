using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	[XmlRoot("CustomersInventoryResponse", Namespace = "http://www.cisco.com/InventoryService")]
	public class CustomersInventoryResponse
	{
		[XmlElement("customerInventory")]
		public List<CustomerInventory> CustomerInventories { get; set; } = null!;

		[XmlElement("responseTimestamp")]
		public DateTime ResponseTimestamp { get; set; }

		[XmlElement("message")]
		public Message Message { get; set; } = null!;
	}
}
