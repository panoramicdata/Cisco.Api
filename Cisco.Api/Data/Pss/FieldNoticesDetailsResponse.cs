using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

/// <summary>
/// The FieldNoticesDetials Response
/// </summary>
[XmlRoot("FNDetailsResponseOutput", Namespace = "http://www.cisco.com/FNAlertService")]
public class FieldNoticesDetailsResponse : PssServiceResponse
{
	/// <summary>
	/// The List of FieldNoticesDetails
	/// </summary>
	[XmlElement("FNDetailsDTO")]
	public List<FieldNoticesDetailsDTO> Details { get; set; } = [];
}