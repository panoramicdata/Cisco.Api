using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	/// <summary>
	/// The ResponseDTO when getting PSIRT
	/// </summary>
	public class PsirtResponseDeviceDTO
	{
		/// <summary>
		/// Id of the device.
		/// </summary>
		[XmlElement("deviceId")]
		public string DeviceId { get; set; } = null!;

		/// <summary>
		/// The Device
		/// </summary>
		[XmlElement("devicePSIRT")]
		public PsirtDevice Device { get; set; } = null!;
	}
}