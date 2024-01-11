using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

/// <summary>
/// The Card Details
/// </summary>
public class CardDetail
{
	/// <summary>
	/// API calls the ID of the card contained inside the device.
	/// </summary>
	[XmlElement("cardId")]
	public string CardId { get; set; } = null!;

	/// <summary>
	/// Device id of the device.
	/// </summary>
	[XmlElement("deviceId")]
	public string DeviceId { get; set; } = null!;

	/// <summary>
	/// Description of the card inside the device.
	/// </summary>
	[XmlElement("cardDescription")]
	public string CardDescription { get; set; } = null!;

	/// <summary>
	/// Identifies which family the card belongs to.
	/// </summary>
	[XmlElement("cardFamily")]
	public string CardFamily { get; set; } = null!;

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
	/// fruFlag: 1: Card is field replaceable unit.
	/// fruFlag: 2: Card is not field replaceable unit.
	/// fruFlag: 0: When element is Chassis or in
	/// Case of card data is not populated.
	/// </summary>
	[XmlElement("fruFlag")]
	public string FruFlag { get; set; } = null!;

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
	/// Slot number where the card is plugged.
	/// </summary>
	[XmlElement("slotNumber")]
	public string SlotNumber { get; set; } = null!;

	/// <summary>
	/// Card’s firmware version number
	/// </summary>
	[XmlElement("firmwareVersionNumber")]
	public string FirmwareVersionNumber { get; set; } = null!;

	/// <summary>
	/// Card’s hardware version number.
	/// </summary>
	[XmlElement("hardwareVersionNumber")]
	public string HardwareVersionNumber { get; set; } = null!;

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