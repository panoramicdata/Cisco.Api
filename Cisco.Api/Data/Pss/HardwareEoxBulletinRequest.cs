using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	/// <summary>
	/// The HardwareEoxBulletin Request
	/// </summary>
	[XmlRoot("HwEoxBulletinRequestInput", Namespace = "http://www.cisco.com/HwEoxAlertService")]
	public class HardwareEoxBulletinRequest
	{
		/// <summary>
		/// Is a list of hardware end-of-life ids and can have
		/// any number of parameters listed in the API service call.
		/// </summary>
		[XmlElement("hwEoxIds")]
		public HardwareEoxIds HardwareEoxIds { get; set; } = null!;
	}
}