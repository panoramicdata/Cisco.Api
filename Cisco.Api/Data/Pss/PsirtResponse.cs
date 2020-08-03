using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	/// <summary>
	/// The Psirt Response
	/// </summary>
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