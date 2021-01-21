using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	/// <summary>
	/// The ResponseDTO when getting SoftwareEox
	/// </summary>
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