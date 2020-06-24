using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	[XmlRoot("PSIRTResponseOutput", Namespace = "http://www.cisco.com/PSIRTAlertService")]
	public class PsirtResponse : PssServiceResponse
	{
		/// <summary>
		/// The list of Devices
		/// </summary>
		[XmlElement("DevicePSIRTResponseDTO")]
		public List<PsirtResponseDeviceDTO> Devices { get; set; } = null!;
	}
}