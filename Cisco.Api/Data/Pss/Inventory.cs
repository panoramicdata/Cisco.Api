using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	[DebuggerDisplay("{Id}:{Name}")]
	[XmlRoot("inventory", Namespace = "http://www.cisco.com/InventoryService")]
	public class Inventory
	{
		/// <summary>
		/// A unique inventory name for a specific uploaded inventory.
		/// </summary>
		[XmlElement("inventoryId")]
		public int Id { get; set; }

		/// <summary>
		/// Inventory Name is name of the file which has the network device information collected by collectors.
		/// </summary>
		[XmlElement("inventoryName")]
		public string Name { get; set; } = null!;

		/// <summary>
		/// Uniquely identifies a collector.
		/// </summary>
		[XmlElement("applianceIds")]
		public string ApplianceIds { get; set; } = null!;

		/// <summary>
		/// The date the inventory was uploaded to the PSS Support Service.
		/// </summary>
		[XmlElement("uploadTime")]
		public DateTime UploadTime { get; set; }
	}
}