using System.Collections.Generic;

namespace Cisco.Api.Data.Pss;

/// <summary>
/// The DeviceconfigsRequest
/// </summary>
public class DeviceConfigsRequest
{
	/// <summary>
	/// This parameter is returned in the API service call getCustomersInventoryIds.
	/// customerId is an id used by Cisco to uniquely identify the company
	/// </summary>
	public string CustomerId { get; set; } = null!;

	/// <summary>
	/// The deviceIds input is limited to a maximum of 5 devices per call.
	/// </summary>
	public List<string> DeviceIds { get; set; } = [];

	/// <summary>
	/// This parameter selects the configuration type you want to return.
	/// The only valid input strings are RUNNING, STARTUP, and BOTH.
	/// </summary>
	public DeviceConfigsConfigType ConfigType { get; set; }
}