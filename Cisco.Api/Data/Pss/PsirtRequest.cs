using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	[XmlRoot("PSIRTRequestInput", Namespace = "http://www.cisco.com/PSIRTAlertService")]
	public class PsirtRequest : PssServiceRequest
	{
	}
}