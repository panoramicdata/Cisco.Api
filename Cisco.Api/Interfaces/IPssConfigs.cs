using Cisco.Api.Data.Pss;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Interfaces;
public interface IPssConfigs
{
	/// <summary>
	/// You can retrieve the running and startup configs for up to 5 devices at a time, in the form of a ZIP file - this is returned as
	/// a MemoryStream so that you can save it to disk or process it further, by passing into ExtractDeviceConfigsZipToDictionaryAsync().
	/// Note that SSH must be enabled for the customer.
	/// </summary>
	/// <param name="deviceConfigsRequest">The request containing customer ID, device IDs, and config type.</param>
	/// <param name="cancellationToken"></param>
	/// <returns>A MemoryStream containing the zipped device configurations.</returns>
	Task<MemoryStream> RetrieveDeviceConfigZipAsync(
		DeviceConfigsRequest deviceConfigsRequest,
		CancellationToken cancellationToken);

	/// <summary>
	/// Extracts a MemoryStream containing device configurations into a dictionary of device configurations.
	/// </summary>
	/// <param name="memoryStream">The MemoryStream containing the zipped device configurations.</param>
	/// <returns>A dictionary with device IDs as keys and DeviceConfigResponse as values.</returns>
	Task<Dictionary<string, DeviceConfigResponse>> ExtractDeviceConfigsZipToDictionaryAsync(
		MemoryStream memoryStream);
}
