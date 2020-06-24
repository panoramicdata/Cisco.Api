using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	[XmlRoot("FNResponseOutput", Namespace = "http://www.cisco.com/FNAlertService")]
	public class FieldNoticesResponse : PssServiceResponse
	{
		/// <summary>
		/// The list of Devices
		/// </summary>
		[XmlElement("DeviceFNResponseDTO")]
		public List<FieldNoticesResponseDeviceDTO> Devices { get; set; } = null!;
	}
}