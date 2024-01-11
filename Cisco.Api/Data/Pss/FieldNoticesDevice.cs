using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

/// <summary>
/// The Device when getting FN Details
/// </summary>
public class FieldNoticesDevice
{
	/// <summary>
	///  Id of the field notice.
	/// </summary>
	[XmlElement("fnId")]
	public string FnId { get; set; } = null!;

	/// <summary>
	///  Match confidence factor for the field notice item.
	/// </summary>
	[XmlElement("matchConfidence")]
	public string MatchConfidence { get; set; } = null!;

	/// <summary>
	/// Match reason factor for the field notice item.
	/// </summary>
	[XmlElement("matchReason")]
	public string MatchReason { get; set; } = null!;
}