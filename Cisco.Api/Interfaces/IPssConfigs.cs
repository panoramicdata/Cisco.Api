using Cisco.Api.Data.Pss;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Interfaces;
public interface IPssConfigs
{
	/// <summary>
	/// This API returns the device configuration for the selected devices if collected by the collector.
	/// This API returns Startup configs and Running configs.
	/// The input data parameters(customerId and deviceIds) are obtained from the data returned in the first two Inventory API service calls.
	/// You must provide the configType you want to retrieve, either RUNNING, STARTUP, or BOTH.
	/// Only output files of up to 100 MB size of the configurations are supported.
	/// </summary>
	/// <param name="customerId">Unique Identifier of the customer.</param>
	/// <param name="deviceIds">Unique Identifier of the customer.</param>
	/// <param name="configType">Unique Identifier of the customer.</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	Task<DeviceConfigResponse> GetDeviceConfigAsync(
		DeviceConfigsRequest deviceConfigsRequest,
		CancellationToken cancellationToken);
}
