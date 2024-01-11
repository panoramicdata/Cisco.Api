using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

/// <summary>
/// The SoftwareEoxBulletin Request
/// </summary>
[XmlRoot("SwEoxBulletinRequestInput", Namespace = "http://www.cisco.com/SwEoxAlertService")]
public class SoftwareEoxBulletinRequest
{
	/// <summary>
	/// Was obtained from the getSoftwareEoX API service call.
	/// </summary>
	[XmlElement("softwareEoxIds")]
	public SoftwareEoxIds SoftwareEoxIds { get; set; } = null!;
}