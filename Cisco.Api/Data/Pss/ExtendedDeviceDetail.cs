using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

public class ExtendedDeviceDetail : DeviceDetail<ExtendedCardDetail>
{
	/// <summary>
	/// The version of the boot code installed on the device.
	/// </summary>
	[XmlElement("bootVersion")]
	public string BootVersion { get; set; } = null!;

	/// <summary>
	/// Specific feature set associated with the software version
	/// (e.g. IP STRING Base With Crypto etc.)
	/// </summary>
	[XmlElement("featureSet")]
	public string FeatureSet { get; set; } = null!;

	/// <summary>
	/// The name of the category of the Cisco device.
	/// For example, Routers, Switches, LAN switches, and so on.
	/// </summary>
	[XmlElement("categoryName")]
	public string CategoryName { get; set; } = null!;

	/// <summary>
	/// The short name of the category of the Cisco device.
	/// </summary>
	[XmlElement("categoryShortName")]
	public string CategoryShortName { get; set; } = null!;

	/// <summary>
	/// Classification of the item as chassis/card/CCM/IPPhone/UCS etc.
	/// </summary>
	[XmlElement("itemType")]
	public string ItemType { get; set; } = null!;

	/// <summary>
	/// SNMP configured location of the device that is available from the inventory upload.
	/// </summary>
	[XmlElement("snmpLocation")]
	public string SnmpLocation { get; set; } = null!;

	/// <summary>
	/// It is the address the device was shipped to.
	/// </summary>
	[XmlElement("shipToAddress")]
	public string ShipToAddress { get; set; } = null!;

	/// <summary>
	/// Address where device is installed. This information comes from the C3 database.
	/// </summary>
	[XmlElement("installAtAddress")]
	public string InstallAtAddress { get; set; } = null!;

	/// <summary>
	/// Entity that is billed for the service coverage.
	/// </summary>
	[XmlElement("billToName")]
	public string BillToName { get; set; } = null!;

	/// <summary>
	/// System contact person as obtained via SNMP.
	/// </summary>
	[XmlElement("systemContact")]
	public string SystemContact { get; set; } = null!;
}
