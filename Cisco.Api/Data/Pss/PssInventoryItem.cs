using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

/// <summary>
/// The product and serial number details common to devices and the cards inside them.
/// </summary>
public abstract class PssInventoryItem
{
	/// <summary>
	/// Base product id of the card.
	/// </summary>
	[XmlElement("productId")]
	public string ProductId { get; set; } = null!;

	/// <summary>
	/// Product description of the card.
	/// </summary>
	[XmlElement("productDescription")]
	public string ProductDescription { get; set; } = null!;

	/// <summary>
	/// Product family the card belongs to.
	/// </summary>
	[XmlElement("productFamily")]
	public string ProductFamily { get; set; } = null!;

	/// <summary>
	/// Original manufacturer’s serial number for the card
	/// </summary>
	[XmlElement("originalSerialNumber")]
	public string OriginalSerialNumber { get; set; } = null!;

	/// <summary>
	/// Serial number that someone validated to be on the card.
	/// </summary>
	[XmlElement("validatedSerialNumber")]
	public string ValidatedSerialNumber { get; set; } = null!;

	/// <summary>
	/// Card’s software version number.
	/// </summary>
	[XmlElement("softwareVersion")]
	public string? SoftwareVersion { get; set; }

	/// <summary>
	/// Original Manufacturer of device.
	/// </summary>
	[XmlElement("manufacturer")]
	public string Manufacturer { get; set; } = null!;

	/// <summary>
	/// Unique association between PID and SN
	/// </summary>
	[XmlElement("c3instanceid")]
	public string C3InstanceId { get; set; } = null!;

	/// <summary>
	/// Ship Date of device
	/// </summary>
	[XmlElement("shipDate")]
	public string ShipDate { get; set; } = null!;
}
