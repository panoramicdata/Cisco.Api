using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
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
		/// Serial number of the device noted on the purchase order.
		/// </summary>
		[XmlElement("originalSerialNumber")]
		public string OriginalSerialNumber { get; set; } = null!;

		/// <summary>
		/// Actual number physically found on the device.
		/// </summary>
		[XmlElement("validatedSerialNumber")]
		public string ValidatedSerialNumber { get; set; } = null!;

		/// <summary>
		/// Refers to the validated product id of the device.
		/// </summary>
		[XmlElement("productId")]
		public string ProductId { get; set; } = null!;

		/// <summary>
		/// Product description of the device.
		/// </summary>
		[XmlElement("productDescription")]
		public string ProductDescription { get; set; } = null!;

		/// <summary>
		/// Product family the device belongs to.
		/// </summary>
		[XmlElement("productFamily")]
		public string ProductFamily { get; set; } = null!;

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
		public DateTime? OriginalInventoryDate;

		/// <summary>
		/// The date the last inventory was performed.
		/// </summary>
		[XmlElement("lastInventoryDate")]
		public DateTime? LastInventoryDate;

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
		/// Software version of the device.
		/// </summary>
		[XmlElement("softwareVersion")]
		public string? SoftwareVersion { get; set; } = null!;

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
		/// Original Manufacturer of device
		/// </summary>
		[XmlElement("manufacturer")]
		public string Manufacturer { get; set; } = null!;

		/// <summary>
		/// Unique association between PID and SN
		/// </summary>
		[XmlElement("c3instanceid")]
		public string C3instanceid { get; set; } = null!;

		/// <summary>
		/// Ship Date of device
		/// </summary>
		[XmlElement("shipDate")]
		public string ShipDate { get; set; } = null!;

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
}