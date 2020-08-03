using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	/// <summary>
	/// The CustomerInventoryDetailPaginatedRequest
	/// </summary>
	[XmlRoot("CustomerInventoryDetailPaginatedRequestInput", Namespace = "http://www.cisco.com/InventoryService")]
	public class CustomerInventoryDetailPaginatedRequest : PssServiceRequest
	{
		/// <summary>
		/// The number of pages to be returned. The first time this
		/// API is called, this value should always be 1
		/// </summary>
		[XmlElement("pageStart")]
		public string PageStart { get; set; } = "1";
	}
}