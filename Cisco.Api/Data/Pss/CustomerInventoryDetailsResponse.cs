using Cisco.Api.Data.Pss;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api;

/// <summary>
/// CustomerInventoryDetails Response
/// </summary>
[XmlRoot("CustomerInventoryDetailResponseOutput", Namespace = "http://www.cisco.com/InventoryService")]
public class CustomerInventoryDetailsResponse : PssServiceResponse
{
	/// <summary>
	/// Device details
	/// </summary>
	[XmlElement("deviceDetail")]
	public List<DeviceDetail> DeviceDetails { get; set; } = null!;
}