using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

/// <summary>
/// The Device when getting HardwareEox
/// </summary>
public class HardwareEoxDevice
{
	/// <summary>
	/// Hardware end-of-life id and can have any number of parameters listed in the API service call.
	/// </summary>
	[XmlElement("hwEoxId")]
	public string HwEoxId { get; set; } = null!;

	/// <summary>
	/// Product id and can have any number of parameters listed in the API service call.
	/// </summary>
	[XmlElement("productId")]
	public string? ProductId { get; set; } = null!;
}