using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	[DebuggerDisplay("{Id}:{Name}")]
	[XmlRoot("inventory", Namespace = "http://www.cisco.com/InventoryService")]
	public class Inventory
	{
		[XmlElement("inventoryId")]
		public int Id { get; set; }

		[XmlElement("inventoryName")]
		public string Name { get; set; } = null!;

		[XmlElement("applianceIds")]
		public string ApplianceIds { get; set; } = null!;

		[XmlElement("uploadTime")]
		public DateTime UploadTime { get; set; }
	}
}