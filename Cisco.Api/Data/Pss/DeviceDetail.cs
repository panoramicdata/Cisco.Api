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
public class DeviceDetail<TCardDetailType> : PssInventoryItem
{
	/// <summary>
	/// Device id of the device contained in a specific
	/// customer inventory
	/// </summary>
	[XmlElement("deviceId")]
	public string DeviceId { get; set; } = null!;

	/// <summary>
	/// Hostname of the device.
	/// </summary>
	[XmlElement("hostName")]
	public string HostName { get; set; } = null!;

	/// <summary>
	/// IP address of the device.
	/// </summary>
	[XmlElement("ipAddress")]
	public string IpAddress { get; set; } = null!;

	/// <summary>
	/// Product name of the device.
	/// </summary>
	[XmlElement("productName")]
	public string ProductName { get; set; } = null!;

	/// <summary>
	/// Product model of the device.
	/// </summary>
	[XmlElement("productModel")]
	public string ProductModel { get; set; } = null!;

	/// <summary>
	/// Element type of the device.
	/// </summary>
	[XmlElement("elementType")]
	public string ElementType { get; set; } = null!;

	/// <summary>
	/// Date the original inventory was performed.
	/// </summary>
	[XmlElement("originalInventoryDate")]
	public DateTime? OriginalInventoryDate { get; set; }

	/// <summary>
	/// The date the last inventory was performed.
	/// </summary>
	[XmlElement("lastInventoryDate")]
	public DateTime? LastInventoryDate { get; set; }

	/// <summary>
	/// Base product id of the device.
	/// </summary>
	[XmlElement("baseProductId")]
	public string BaseProductId { get; set; } = null!;

	/// <summary>
	/// Amount of memory the device has installed.
	/// </summary>
	[XmlElement("installedMemory")]
	public string InstalledMemory { get; set; } = null!;

	/// <summary>
	/// Hardware version of the device.
	/// </summary>
	[XmlElement("hardWareVersion")]
	public string HardWareVersion { get; set; } = null!;

	/// <summary>
	/// OS type of the software.
	/// </summary>
	[XmlElement("softwareType")]
	public string SoftwareType { get; set; } = null!;

	/// <summary>
	/// Amount of flash memory the device has installed.
	/// </summary>
	[XmlElement("flashMemory")]
	public string FlashMemory { get; set; } = null!;

	/// <summary>
	/// Operating System version of the device.
	/// </summary>
	[XmlElement("osVersion")]
	public string OsVersion { get; set; } = null!;

	/// <summary>
	/// Todo: from the Response this looks like an Enum,
	/// not mentioned in the docs (PA)
	/// </summary>
	[XmlElement("selectionType")]
	public string SelectionType { get; set; } = null!;

	/// <summary>
	/// Todo: from the Response this looks like an Enum,
	/// not mentioned in the docs (PA)
	/// </summary>
	[XmlElement("equipmentType")]
	public string EquipmentType { get; set; } = null!;

	/// <summary>
	/// Cards detail
	/// </summary>
	[XmlElement("cardDetail")]
	public List<TCardDetailType> CardDetails { get; set; } = null!;
}
