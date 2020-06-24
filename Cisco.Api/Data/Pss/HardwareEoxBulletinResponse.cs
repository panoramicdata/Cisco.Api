using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	[XmlRoot("HwEoxBulletinResponseOutput", Namespace = "http://www.cisco.com/HwEoxAlertService")]
	public class HardwareEoxBulletinResponse : PssServiceResponse
	{
		/// <summary>
		/// The List of HardwareEoxBulletin DTOs
		/// </summary>
		[XmlElement("HwEoxBulletinDTO")]
		public List<HardwareEoxBulletin> HardwareEoxBulletin { get; set; } = null!;
	}
}