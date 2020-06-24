using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	[XmlRoot("SwEoxBulletinRequestInput", Namespace = "http://www.cisco.com/SwEoxAlertService")]
	public class SoftwareEoxBulletinRequest
	{
		/// <summary>
		/// Was obtained from the getSoftwareEoX API service call.
		/// </summary>
		[XmlElement("softwareEoxIds")]
		public List<SoftwareEoxId> SoftwareEoxIds { get; set; } = null!;
	}
}