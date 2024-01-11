using System.Diagnostics;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

/// <summary>
/// The Customer
/// </summary>
[DebuggerDisplay("{Id}:{Name}")]
[XmlRoot("customer", Namespace = "http://www.cisco.com/InventoryService")]
public class Customer
{
	/// <summary>
	/// An id used by Cisco to uniquely identify the company
	/// </summary>
	[XmlElement("customerId")]
	public string Id { get; set; } = null!;

	/// <summary>
	/// The name of the customer for whose network data is being retrieved via the API’s.
	/// </summary>
	[XmlElement("customerName")]
	public string Name { get; set; } = null!;
}