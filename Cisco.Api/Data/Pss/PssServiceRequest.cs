using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	public abstract class PssServiceRequest
	{
		/// <summary>
		/// This parameter is returned in the API service call getCustomersInventoryIds.
		/// customerId is an id used by Cisco to uniquely identify
		/// the company
		/// </summary>
		[XmlElement("customerId")]
		public string CustomerId { get; set; } = null!;

		/// <summary>
		/// This parameter is returned in the API service call
		/// getCustomersInventoryIds.
		/// inventoryId identifies the inventory whose data will be
		/// accessed for the chassis & card level details.
		/// </summary>
		[XmlElement("inventoryId")]
		public string InventoryId { get; set; } = null!;

		/// <summary>
		/// The deviceIds input can be zero or more.  When not
		/// providing the deviceIds, the api will return a full
		/// collection of data for that given InventoryID, for that
		/// given CustomerId, of that given PartyGUID; all the
		/// devices collected for that inventory will be returned.
		/// Note When the partners issue an Inventory Service
		/// API – GetCustomerInventoryX, or
		/// GestCustomerExtendedInventoryDetails,
		/// there are two responses that are labeled
		/// deviceId, one for the Chassis(under
		/// DeviceDetails), and one for the cards(under
		/// CardsDetail). Some of the Alert APIs can use
		/// the deviceId as an optional input.For APIs
		/// such as getFNDetails, getPSIRT, getIS,
		/// getHwEox, getSoftwareEox, getFN, and
		/// getContractCoverage partner MUST use the
		/// deviceId from the DeviceDetails response.If
		/// the deviceID is used from CardDetails, then
		/// the API will not return the proper data
		/// </summary>
		[XmlElement("deviceIds")]
		public List<string>? DeviceIds { get; set; }
	}
}
