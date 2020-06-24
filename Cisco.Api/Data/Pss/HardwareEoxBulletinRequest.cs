using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	[XmlRoot("HwEoxBulletinRequestInput", Namespace = "http://www.cisco.com/HwEoxAlertService")]
	public class HardwareEoxBulletinRequest
	{
		/// <summary>
		/// Is the hardware end-of-life id and can have
		/// any number of parameters listed in the API service call.
		/// </summary>
		[XmlElement("hwEoxIds")]
		public List<HardwareEoxId> HardwareEoxIds { get; set; } = null!;
	}
}