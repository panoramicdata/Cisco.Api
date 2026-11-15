using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

/// <summary>
/// The Card Details
/// </summary>
public class CardDetail : PssInventoryItem
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
	/// fruFlag: 1: Card is field replaceable unit.
	/// fruFlag: 2: Card is not field replaceable unit.
	/// fruFlag: 0: When element is Chassis or in
	/// Case of card data is not populated.
	/// </summary>
	[XmlElement("fruFlag")]
	public string FruFlag { get; set; } = null!;

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
}
