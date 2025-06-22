using System;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

/// <summary>
/// The SoftwareEoxBulletin DTO
/// </summary>
public class SoftwareEoxBulletin
{
	/// <summary>
	/// Software end-of-life bulletin number.
	/// </summary>
	[XmlElement("bulletinNumber")]
	public string BulletinNumber { get; set; } = null!;

	/// <summary>
	/// URL for the end-of-life bulletin.
	/// </summary>
	[XmlElement("bulletinURL")]
	public string BulletinUrl { get; set; } = null!;

	/// <summary>
	/// Indicates the End-of-Engineering (EOE) Date, if reached, otherwise the date is blank.
	/// </summary>
	[XmlElement("endOfEngineeringDate")]
	public DateTime? EndOfEngineeringDate { get; set; }

	/// <summary>
	/// Indicates the End-of-Life (EOL) Date, if reached,
	/// otherwise the date is blank.This is the date
	/// the document announces the end of sale,
	/// and the end of life of a product is distributed to the general public.
	/// </summary>
	[XmlElement("endOfLifeDate")]
	public DateTime? EndOfLifeDate { get; set; }

	/// <summary>
	/// Indicates the End-of-Sale (EOS) Date, if reached,
	/// otherwise the date is blank.This is the last date to order the product through Cisco point-of-sale mechanisms.
	/// The product is no longer for sale after this date.
	/// </summary>
	[XmlElement("endOfSaleDate")]
	public DateTime? EndOfSaleDate { get; set; }

	/// <summary>
	/// Id of the software slated for end of life.
	/// </summary>
	[XmlElement("softwareEoXId")]
	public string SoftwareEoxId { get; set; } = null!;
}