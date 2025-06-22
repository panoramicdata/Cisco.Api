using System;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

/// <summary>
/// The HardwareEoxBulletin
/// </summary>
public class HardwareEoxBulletin
{
	/// <summary>
	/// Name of the hardware bulletin.
	/// </summary>
	[XmlElement("bulletinName")]
	public string BulletinName { get; set; } = null!;

	/// <summary>
	/// Number of the hardware bulletin.
	/// </summary>
	[XmlElement("bulletinNumber")]
	public string BulletinNumber { get; set; } = null!;

	/// <summary>
	/// Product id of the hardware bulletin.
	/// </summary>
	[XmlElement("bulletinPID")]
	public string BulletinPID { get; set; } = null!;

	/// <summary>
	/// URL of the hardware bulletin
	/// </summary>
	[XmlElement("bulletinURL")]
	public string BulletinUrl { get; set; } = null!;

	/// <summary>
	/// Indicates the End of Routine Failure Analysis Date,
	/// which is the last-possible date a routine failure analysis
	/// may be performed to determine the cause of product failure or defect.
	/// </summary>
	[XmlElement("endOfHardwareRoutineFailureAnalysisDate")]
	public DateTime? EndOfHardwareRoutineFailureAnalysisDate { get; set; }

	/// <summary>
	/// Indicates the End of Service Contract Renewal(EoSCR).
	/// This the last date to extend or renew a service contract for the product.
	/// The extension or renewal period may not extend beyond the last date of support.
	/// </summary>
	[XmlElement("endOfHardwareServiceContractRenewalDate")]
	public DateTime? EndOfHardwareServiceContractRenewalDate { get; set; }

	/// <summary>
	/// Indicates the End-of-Last Date of Support for the device,
	/// which is the last date to receive service and support for the product.
	/// After this date, all support services for the product are unavailable,
	/// and the product becomes obsolete.
	/// </summary>
	[XmlElement("endOfLastDateOfSupport")]
	public DateTime? EndOfLastDateOfSupport { get; set; }

	/// <summary>
	/// Indicates the End-of-Life Announcement Date, which is the date the document
	/// announces the end of sale, and end of life of a product that is distributed to the general public.
	/// </summary>
	[XmlElement("endOfLifeExternalAnnouncementDate")]
	public DateTime? EndOfLifeExternalAnnouncementDate { get; set; }

	/// <summary>
	/// Indicates the End-of-Sale Date, which is the last date to order the product
	/// through Cisco point-of-sale mechanisms.The product is no longer for sale after this date.
	/// </summary>
	[XmlElement("endOfSaleDate")]
	public DateTime? EndOfSaleDate { get; set; }

	/// <summary>
	/// Indicates the End of Software Maintenance Date,
	/// which is the last date that Cisco Engineering may release any final software maintenance releases
	/// or bug fixes. After this date, Cisco Engineering will no longer
	/// develop, repair, maintain, or test the product software.
	/// </summary>
	[XmlElement("endOfSoftwareMaintenanceReleasesDate")]
	public DateTime? EndOfSoftwareMaintenanceReleasesDate { get; set; }

	/// <summary>
	/// Hardware end-of-life id obtained from the previous API call.
	/// </summary>
	[XmlElement("hwEoXId")]
	public string HwEoxId { get; set; } = null!;

	/// <summary>
	/// SVC attach end date.
	/// </summary>
	[XmlElement("svcAttachEndDate")]
	public DateTime? SvcAttachEndDate { get; set; }
}