using Cisco.Api.Data.Pss;
using System.Xml.Serialization;

namespace Cisco.Api
{
	/// <summary>
	/// CustomerExtendedInventoryDetails Request
	/// </summary>
	[XmlRoot("CustomerExtendedInventoryDetailRequestInput", Namespace = "http://www.cisco.com/InventoryService")]
	public class CustomerExtendedInventoryDetailsRequest : PssServiceRequest
	{
		[XmlElement("pageStart")]
		public int PageStart { get; set; } = 1;
	}
}