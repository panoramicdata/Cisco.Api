using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	[XmlRoot("HwEoxRequestInput", Namespace = "http://www.cisco.com/HwEoxAlertService")]
	public class HardwareEoxRequest : PssServiceRequest
	{
	}
}