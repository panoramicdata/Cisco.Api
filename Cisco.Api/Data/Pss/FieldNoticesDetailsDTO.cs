using System;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

/// <summary>
/// The ResponseDTO when getting FieldNoticesDetails
/// </summary>
public class FieldNoticesDetailsDTO
{
	/// <summary>
	/// Id of the field notice.
	/// </summary>
	[XmlElement("fnId")]
	public string FnId { get; set; } = null!;

	/// <summary>
	/// Caveat of the field notice.
	/// </summary>
	[XmlElement("caveat")]
	public string Caveat { get; set; } = null!;

	/// <summary>
	/// Distribution code for the field notice.
	/// </summary>
	[XmlElement("distributionCode")]
	public string DistributionCode { get; set; } = null!;

	/// <summary>
	/// Name of the field notice.
	/// </summary>
	[XmlElement("fieldNoticeName")]
	public string FieldNoticeName { get; set; } = null!;

	/// <summary>
	/// Type of code for the field notice.
	/// </summary>
	[XmlElement("fieldNoticeTypeCode")]
	public string FieldNoticeTypeCode { get; set; } = null!;

	/// <summary>
	/// URL for the field notice.
	/// </summary>
	[XmlElement("fieldNoticeURL")]
	public string FieldNoticeURL { get; set; } = null!;

	/// <summary>
	/// Date the field notice was first published.
	/// </summary>
	[XmlElement("firstPublishDate")]
	public DateTime FirstPublishDate { get; set; }

	/// <summary>
	/// The available serial number codes for the field notice.
	/// </summary>
	[XmlElement("isSerialNumberAvailableCode")]
	public string IsSerialNumberAvailableCode { get; set; } = null!;

	/// <summary>
	/// Date the field notice was last revised.
	/// </summary>
	[XmlElement("lastRevisionDate")]
	public DateTime LastRevisionDate { get; set; }

	/// <summary>
	/// User id of the person who published the field notice.
	/// </summary>
	[XmlElement("publishUserId")]
	public string PublishUserId { get; set; } = null!;

	/// <summary>
	/// Current status of the field notice.
	/// </summary>
	[XmlElement("status")]
	public string Status { get; set; } = null!;
}