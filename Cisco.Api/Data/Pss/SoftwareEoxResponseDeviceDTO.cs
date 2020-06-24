using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	public class SoftwareEoxResponseDeviceDTO
	{
		/// <summary>
		/// Device id of the device.
		/// </summary>
		[XmlElement("deviceId")]
		public string DeviceId { get; set; } = null!;

		/// <summary>
		/// The DeviceDTO
		/// </summary>
		[XmlElement("deviceSwEox")]
		public SoftwareEoxDevice Device { get; set; } = null!;
	}
}