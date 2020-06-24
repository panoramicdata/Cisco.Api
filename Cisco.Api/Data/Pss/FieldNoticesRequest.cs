using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	[XmlRoot("FNRequestInput", Namespace = "http://www.cisco.com/FNAlertService")]
	public class FieldNoticesRequest : PssServiceRequest
	{
	}
}