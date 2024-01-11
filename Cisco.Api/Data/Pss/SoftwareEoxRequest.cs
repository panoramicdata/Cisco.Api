using Cisco.Api.Data.Pss;
using System.Xml.Serialization;

namespace Cisco.Api;

/// <summary>
/// The SoftwareEox Request
/// </summary>
[XmlRoot("SwEoxRequestInput", Namespace = "http://www.cisco.com/SwEoxAlertService")]
public class SoftwareEoxRequest : PssServiceRequest
{
}