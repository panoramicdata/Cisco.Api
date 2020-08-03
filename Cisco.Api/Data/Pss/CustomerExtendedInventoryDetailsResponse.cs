using Cisco.Api.Data.Pss;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api
{
	/// <summary>
	/// CustomerExtendedInventoryDetails Response
	/// </summary>
	[XmlRoot("CustomerExtendedInventoryDetailResponseOutput", Namespace = "http://www.cisco.com/InventoryService")]
	public class CustomerExtendedInventoryDetailsResponse : PssServiceResponse
	{
		/// <summary>
		/// Page information
		/// </summary>
		[XmlElement("pages")]
		public Pages Pages { get; set; } = null!;

		/// <summary>
		/// Device details
		/// </summary>
		[XmlElement("deviceDetail")]
		public List<ExtendedDeviceDetail> DeviceDetails { get; set; } = null!;
	}
}