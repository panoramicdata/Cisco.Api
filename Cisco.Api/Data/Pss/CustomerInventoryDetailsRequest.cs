using Cisco.Api.Data.Pss;
using System.Xml.Serialization;

namespace Cisco.Api
{
	[XmlRoot("CustomerInventoryDetailRequestInput", Namespace = "http://www.cisco.com/InventoryService")]
	public class CustomerInventoryDetailsRequest : PssServiceRequest
	{
	}
}