using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	/// <summary>
	/// The HardwareEox Request
	/// </summary>
	[XmlRoot("HwEoxRequestInput", Namespace = "http://www.cisco.com/HwEoxAlertService")]
	public class HardwareEoxRequest : PssServiceRequest
	{
	}
}