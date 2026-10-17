using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

/// <summary>
/// Represents the device detail.
/// </summary>
public class DeviceDetail : DeviceDetail<CardDetail>
{
}

/// <summary>
/// The DeviceDetail
/// </summary>
[DebuggerDisplay("{DeviceId}:{ValidatedSerialNumber}")]
[XmlRoot("deviceDetail", Namespace = "http://www.cisco.com/InventoryService")]
public class DeviceDetail<TCardDetailType>
{
	private string deviceId = null!;
	private string hostName = null!;
	private string ipAddress = null!;
	private string originalSerialNumber = null!;
	private string validatedSerialNumber = null!;
	private string productId = null!;
	private string productDescription = null!;
	private string productFamily = null!;
	private string productName = null!;
	private string productModel = null!;
	private string elementType = null!;
	private DateTime? originalInventoryDate;
	private DateTime? lastInventoryDate;
	private string baseProductId = null!;
	private string installedMemory = null!;
	private string hardWareVersion = null!;
	private string? softwareVersion;
	private string softwareType = null!;
	private string flashMemory = null!;
	private string manufacturer = null!;
	private string c3InstanceId = null!;
	private string shipDate = null!;
	private string osVersion = null!;
	private string selectionType = null!;
	private string equipmentType = null!;
	private List<TCardDetailType> cardDetails = null!;

	/// <summary>
	/// Device id of the device contained in a specific
	/// customer inventory
	/// </summary>
	[XmlElement("deviceId")]
	public string DeviceId
	{
		get => deviceId;
		set => deviceId = value;
	}

	/// <summary>
	/// Hostname of the device.
	/// </summary>
	[XmlElement("hostName")]
	public string HostName
	{
		get => hostName;
		set => hostName = value;
	}

	/// <summary>
	/// IP address of the device.
	/// </summary>
	[XmlElement("ipAddress")]
	public string IpAddress
	{
		get => ipAddress;
		set => ipAddress = value;
	}

	/// <summary>
	/// Serial number of the device noted on the purchase order.
	/// </summary>
	[XmlElement("originalSerialNumber")]
	public string OriginalSerialNumber
	{
		get => originalSerialNumber;
		set => originalSerialNumber = value;
	}

	/// <summary>
	/// Actual number physically found on the device.
	/// </summary>
	[XmlElement("validatedSerialNumber")]
	public string ValidatedSerialNumber
	{
		get => validatedSerialNumber;
		set => validatedSerialNumber = value;
	}

	/// <summary>
	/// Refers to the validated product id of the device.
	/// </summary>
	[XmlElement("productId")]
	public string ProductId
	{
		get => productId;
		set => productId = value;
	}

	/// <summary>
	/// Product description of the device.
	/// </summary>
	[XmlElement("productDescription")]
	public string ProductDescription
	{
		get => productDescription;
		set => productDescription = value;
	}

	/// <summary>
	/// Product family the device belongs to.
	/// </summary>
	[XmlElement("productFamily")]
	public string ProductFamily
	{
		get => productFamily;
		set => productFamily = value;
	}

	/// <summary>
	/// Product name of the device.
	/// </summary>
	[XmlElement("productName")]
	public string ProductName
	{
		get => productName;
		set => productName = value;
	}

	/// <summary>
	/// Product model of the device.
	/// </summary>
	[XmlElement("productModel")]
	public string ProductModel
	{
		get => productModel;
		set => productModel = value;
	}

	/// <summary>
	/// Element type of the device.
	/// </summary>
	[XmlElement("elementType")]
	public string ElementType
	{
		get => elementType;
		set => elementType = value;
	}

	/// <summary>
	/// Date the original inventory was performed.
	/// </summary>
	[XmlElement("originalInventoryDate")]
	public DateTime? OriginalInventoryDate
	{
		get => originalInventoryDate;
		set => originalInventoryDate = value;
	}

	/// <summary>
	/// The date the last inventory was performed.
	/// </summary>
	[XmlElement("lastInventoryDate")]
	public DateTime? LastInventoryDate
	{
		get => lastInventoryDate;
		set => lastInventoryDate = value;
	}

	/// <summary>
	/// Base product id of the device.
	/// </summary>
	[XmlElement("baseProductId")]
	public string BaseProductId
	{
		get => baseProductId;
		set => baseProductId = value;
	}

	/// <summary>
	/// Amount of memory the device has installed.
	/// </summary>
	[XmlElement("installedMemory")]
	public string InstalledMemory
	{
		get => installedMemory;
		set => installedMemory = value;
	}

	/// <summary>
	/// Hardware version of the device.
	/// </summary>
	[XmlElement("hardWareVersion")]
	public string HardWareVersion
	{
		get => hardWareVersion;
		set => hardWareVersion = value;
	}

	/// <summary>
	/// Software version of the device.
	/// </summary>
	[XmlElement("softwareVersion")]
	public string? SoftwareVersion
	{
		get => softwareVersion;
		set => softwareVersion = value;
	}

	/// <summary>
	/// OS type of the software.
	/// </summary>
	[XmlElement("softwareType")]
	public string SoftwareType
	{
		get => softwareType;
		set => softwareType = value;
	}

	/// <summary>
	/// Amount of flash memory the device has installed.
	/// </summary>
	[XmlElement("flashMemory")]
	public string FlashMemory
	{
		get => flashMemory;
		set => flashMemory = value;
	}

	/// <summary>
	/// Original Manufacturer of device
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

	/// <summary>
	/// Operating System version of the device.
	/// </summary>
	[XmlElement("osVersion")]
	public string OsVersion
	{
		get => osVersion;
		set => osVersion = value;
	}

	/// <summary>
	/// Todo: from the Response this looks like an Enum,
	/// not mentioned in the docs (PA)
	/// </summary>
	[XmlElement("selectionType")]
	public string SelectionType
	{
		get => selectionType;
		set => selectionType = value;
	}

	/// <summary>
	/// Todo: from the Response this looks like an Enum,
	/// not mentioned in the docs (PA)
	/// </summary>
	[XmlElement("equipmentType")]
	public string EquipmentType
	{
		get => equipmentType;
		set => equipmentType = value;
	}

	/// <summary>
	/// Cards detail
	/// </summary>
	[XmlElement("cardDetail")]
	public List<TCardDetailType> CardDetails
	{
		get => cardDetails;
		set => cardDetails = value;
	}
}