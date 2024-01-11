using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

/// <summary>
/// The FieldNoticesDetails Request
/// </summary>
[XmlRoot("FNDetailsRequestInput", Namespace = "http://www.cisco.com/FNAlertService")]
public class FieldNoticesDetailsRequest
{
	/// <summary>
	/// Was obtained from the getFN API service call.
	/// </summary>
	[XmlArray("fnIds")]
	[XmlArrayItem("fnId")]
	public List<string> Ids { get; set; } = null!;
}