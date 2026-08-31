using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

/// <summary>
/// The SoftwareEoxBulletin Response
/// </summary>
[XmlRoot("SwEoxBulletinResponseOutput", Namespace = "http://www.cisco.com/SwEoxAlertService")]
public class SoftwareEoxBulletinResponse : PssServiceResponse
{
	/// <summary>
	/// Gets or sets the bulletins.
	/// </summary>
	[XmlElement("SwEoxBulletinDTO")]
	public List<SoftwareEoxBulletin> Bulletins { get; set; } = null!;
}