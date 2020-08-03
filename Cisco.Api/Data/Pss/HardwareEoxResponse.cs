using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	[XmlRoot("HwEoxResponseOutput", Namespace = "http://www.cisco.com/HwEoxAlertService")]
	public class HardwareEoxResponse : PssServiceResponse
	{
		/// <summary>
		/// The list of Devices
		/// </summary>
		[XmlElement("DeviceHwEoxResponseDTO")]
		public List<HardwareEoxResponseDeviceDTO> Devices { get; set; } = null!;
	}
}