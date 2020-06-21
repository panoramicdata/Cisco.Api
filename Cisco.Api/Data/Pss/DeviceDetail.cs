using System.Diagnostics;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	[DebuggerDisplay("{DeviceId}:{ValidatedSerialNumber}")]
	[XmlRoot("deviceDetail", Namespace = "http://www.cisco.com/InventoryService")]
	public class DeviceDetail
	{
		/// <summary>
		/// Device id of the device contained in a specific
		/// customer inventory
		/// </summary>
		[XmlElement("deviceId")]
		public string DeviceId { get; set; } = null!;

		/// <summary>
		/// Hostname of the device.
		/// </summary>
		[XmlElement("pageCurrent")]
		public string PageCurrent { get; set; } = null!;

		/// <summary>
		/// IP address of the device.
		/// </summary>
		[XmlElement("ipAddress")]
		public string IpAddress { get; set; } = null!;

		/// <summary>
		/// Serial number of the device noted on the purchase order.
		/// </summary>
		[XmlElement("originalSerialNumber")]
		public string OriginalSerialNumber { get; set; } = null!;

		/// <summary>
		/// Actual number physically found on the device.
		/// </summary>
		[XmlElement("validatedSerialNumber")]
		public string ValidatedSerialNumber { get; set; } = null!;
	}
}