using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

/// <summary>
/// The FieldNotices Request
/// </summary>
[XmlRoot("FNRequestInput", Namespace = "http://www.cisco.com/FNAlertService")]
public class FieldNoticesRequest : PssServiceRequest
{
}