using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

/// <summary>
/// The HardwareEox Response
/// </summary>
[XmlRoot("HwEoxResponseOutput", Namespace = "http://www.cisco.com/HwEoxAlertService")]
public class HardwareEoxResponse : PssServiceResponse
{
	/// <summary>
	/// The list of Devices
	/// </summary>
	[XmlElement("DeviceHwEoxResponseDTO")]
	public List<HardwareEoxResponseDeviceDTO> Devices { get; set; } = null!;
}