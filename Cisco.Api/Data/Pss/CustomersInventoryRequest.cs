using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	/// <summary>
	/// CustomersInventory Request
	/// </summary>
	[XmlRoot("CustomerInventoryRequest", Namespace = "http://www.cisco.com/InventoryService")]
	public class CustomersInventoryRequest
	{
		/// <summary>
		/// Input can be null or a specified number.
		/// If the API input is null, all of the registered
		/// customers will be returned in the output.
		/// </summary>
		[XmlElement("customerIds")]
		public List<string> CustomerIds { get; set; } = null!;
	}
}
