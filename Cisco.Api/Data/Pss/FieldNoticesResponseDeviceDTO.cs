using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	public class FieldNoticesResponseDeviceDTO
	{
		/// <summary>
		///  Id of the device.
		/// </summary>
		[XmlElement("deviceId")]
		public string DeviceId { get; set; } = null!;

		[XmlElement("deviceFN")]
		public FieldNoticesDevice Device { get; set; } = null!;
	}
}