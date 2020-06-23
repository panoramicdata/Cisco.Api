using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	[XmlRoot("CustomerInventoryDetailPaginatedResponseOutput", Namespace = "http://www.cisco.com/InventoryService")]
	public class CustomerInventoryDetailPaginatedResponse : PssServiceResponse
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
		public List<DeviceDetail> DeviceDetails { get; set; } = null!;
	}
}