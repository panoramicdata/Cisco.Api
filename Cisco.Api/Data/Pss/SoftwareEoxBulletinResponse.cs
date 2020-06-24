using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	[XmlRoot("SwEoxBulletinResponseOutput", Namespace = "http://www.cisco.com/SwEoxAlertService")]
	public class SoftwareEoxBulletinResponse : PssServiceResponse
	{
		[XmlElement("SwEoxBulletinDTO")]
		public List<SoftwareEoxBulletin> Bulletins { get; set; } = null!;
	}
}