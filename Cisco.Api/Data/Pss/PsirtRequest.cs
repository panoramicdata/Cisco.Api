using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	/// <summary>
	/// The Psirt Request
	/// </summary>
	[XmlRoot("PSIRTRequestInput", Namespace = "http://www.cisco.com/PSIRTAlertService")]
	public class PsirtRequest : PssServiceRequest
	{
	}
}