using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	public class HardwareEoxResponseDeviceDTO
	{
		/// <summary>
		/// Specific device id, selected from the previous API call.
		/// </summary>
		[XmlElement("deviceId")]
		public string DeviceId { get; set; } = null!;

		/// <summary>
		/// The Device
		/// </summary>
		[XmlElement("deviceHwEox")]
		public HardwareEoxDevice Device { get; set; } = null!;
	}
}