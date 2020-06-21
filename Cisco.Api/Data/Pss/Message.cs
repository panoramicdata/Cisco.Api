using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	[XmlRoot("message", Namespace = "http://www.cisco.com/InventoryService")]
	public class Message
	{
		[XmlElement("messageType")]
		public string MessageType { get; set; } = null!;

		[XmlElement("messageDetail")]
		public string MessageDetail { get; set; } = null!;
	}
}