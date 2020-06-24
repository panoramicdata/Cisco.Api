using Cisco.Api.Data.Pss;
using System.Xml.Serialization;

namespace Cisco.Api
{
	[XmlRoot("SwEoxRequestInput", Namespace = "http://www.cisco.com/SwEoxAlertService")]
	public class SoftwareEoxRequest : PssServiceRequest
	{
	}
}