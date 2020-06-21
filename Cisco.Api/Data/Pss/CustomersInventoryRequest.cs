using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	[XmlRoot("CustomerInventoryRequest", Namespace = "http://www.cisco.com/InventoryService")]
	public class CustomersInventoryRequest
	{
		[XmlElement("customerIds")]
		public string CustomerIds { get; set; } = string.Empty;
	}
}
