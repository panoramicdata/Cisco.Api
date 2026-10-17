using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

/// <summary>
/// The Card Details
/// </summary>
public class CardDetail
{
	private string cardId = null!;
	private string deviceId = null!;
	private string cardDescription = null!;
	private string cardFamily = null!;
	private string productId = null!;
	private string productDescription = null!;
	private string productFamily = null!;
	private string fruFlag = null!;
	private string originalSerialNumber = null!;
	private string validatedSerialNumber = null!;
	private string slotNumber = null!;
	private string firmwareVersionNumber = null!;
	private string hardwareVersionNumber = null!;
	private string? softwareVersion;
	private string manufacturer = null!;
	private string c3InstanceId = null!;
	private string shipDate = null!;

	/// <summary>
	/// API calls the ID of the card contained inside the device.
	/// </summary>
	[XmlElement("cardId")]
	public string CardId
	{
		get => cardId;
		set => cardId = value;
	}

	/// <summary>
	/// Device id of the device.
	/// </summary>
	[XmlElement("deviceId")]
	public string DeviceId
	{
		get => deviceId;
		set => deviceId = value;
	}

	/// <summary>
	/// Description of the card inside the device.
	/// </summary>
	[XmlElement("cardDescription")]
	public string CardDescription
	{
		get => cardDescription;
		set => cardDescription = value;
	}

	/// <summary>
	/// Identifies which family the card belongs to.
	/// </summary>
	[XmlElement("cardFamily")]
	public string CardFamily
	{
		get => cardFamily;
		set => cardFamily = value;
	}

	/// <summary>
	/// Base product id of the card.
	/// </summary>
	[XmlElement("productId")]
	public string ProductId
	{
		get => productId;
		set => productId = value;
	}

	/// <summary>
	/// Product description of the card.
	/// </summary>
	[XmlElement("productDescription")]
	public string ProductDescription
	{
		get => productDescription;
		set => productDescription = value;
	}

	/// <summary>
	/// Product family the card belongs to.
	/// </summary>
	[XmlElement("productFamily")]
	public string ProductFamily
	{
		get => productFamily;
		set => productFamily = value;
	}

	/// <summary>
	/// fruFlag: 1: Card is field replaceable unit.
	/// fruFlag: 2: Card is not field replaceable unit.
	/// fruFlag: 0: When element is Chassis or in
	/// Case of card data is not populated.
	/// </summary>
	[XmlElement("fruFlag")]
	public string FruFlag
	{
		get => fruFlag;
		set => fruFlag = value;
	}

	/// <summary>
	/// Original manufacturer’s serial number for the card
	/// </summary>
	[XmlElement("originalSerialNumber")]
	public string OriginalSerialNumber
	{
		get => originalSerialNumber;
		set => originalSerialNumber = value;
	}

	/// <summary>
	/// Serial number that someone validated to be on the card.
	/// </summary>
	[XmlElement("validatedSerialNumber")]
	public string ValidatedSerialNumber
	{
		get => validatedSerialNumber;
		set => validatedSerialNumber = value;
	}

	/// <summary>
	/// Slot number where the card is plugged.
	/// </summary>
	[XmlElement("slotNumber")]
	public string SlotNumber
	{
		get => slotNumber;
		set => slotNumber = value;
	}

	/// <summary>
	/// Card’s firmware version number
	/// </summary>
	[XmlElement("firmwareVersionNumber")]
	public string FirmwareVersionNumber
	{
		get => firmwareVersionNumber;
		set => firmwareVersionNumber = value;
	}

	/// <summary>
	/// Card’s hardware version number.
	/// </summary>
	[XmlElement("hardwareVersionNumber")]
	public string HardwareVersionNumber
	{
		get => hardwareVersionNumber;
		set => hardwareVersionNumber = value;
	}

	/// <summary>
	/// Card’s software version number.
	/// </summary>
	[XmlElement("softwareVersion")]
	public string? SoftwareVersion
	{
		get => softwareVersion;
		set => softwareVersion = value;
	}

	/// <summary>
	/// Original Manufacturer of device.
	/// </summary>
	[XmlElement("manufacturer")]
	public string Manufacturer
	{
		get => manufacturer;
		set => manufacturer = value;
	}

	/// <summary>
	/// Unique association between PID and SN
	/// </summary>
	[XmlElement("c3instanceid")]
	public string C3InstanceId
	{
		get => c3InstanceId;
		set => c3InstanceId = value;
	}

	/// <summary>
	/// Ship Date of device
	/// </summary>
	[XmlElement("shipDate")]
	public string ShipDate
	{
		get => shipDate;
		set => shipDate = value;
	}
}