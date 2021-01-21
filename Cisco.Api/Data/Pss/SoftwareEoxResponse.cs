using Cisco.Api.Data.Pss;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api
{
	/// <summary>
	/// The SoftwareEox Response
	/// </summary>
	[XmlRoot("SwEoxResponseOutput", Namespace = "http://www.cisco.com/SwEoxAlertService")]
	public class SoftwareEoxResponse : PssServiceResponse
	{
		/// <summary>
		/// A list of Devices
		/// </summary>
		[XmlElement("DeviceSwEoxResponseDTO")]
		public List<SoftwareEoxResponseDeviceDTO> Devices { get; set; } = null!;
	}
}