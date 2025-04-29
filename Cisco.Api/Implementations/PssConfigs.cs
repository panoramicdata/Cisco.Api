using Cisco.Api.Data.Pss;
using Cisco.Api.Interfaces;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Implementations;
internal class PssConfigs : IPssConfigs
{
	private HttpClient restHttpClient;

	public PssConfigs(HttpClient restHttpClient)
	{
		this.restHttpClient = restHttpClient;
	}

	public async Task<MemoryStream> RetrieveDeviceConfigZipAsync(DeviceConfigsRequest deviceConfigsRequest, CancellationToken cancellationToken = default)
	{
		// You can retrieve the running and startup configs for up to 5 devices at a time, in the form of a ZIP file.
		// Docs: https://docs.cloudapps.cisco.com/pss/APIDevGuide/service_inventory.html#getDeviceConfig

		// This method retrieves the zipped config file, and returns it as as a MemoryStream so that user can opt to dump the zip
		// or pass it to ConvertDeviceConfigsZipToObjectAsync() the contents immediately.


		var customerId = deviceConfigsRequest.CustomerId;
		var deviceIds = string.Join(",", deviceConfigsRequest.DeviceIds);
		var configType = deviceConfigsRequest.ConfigType;

		if (deviceIds.Length == 0)
		{
			throw new Exception("No device IDs provided.");
		}

		if (deviceIds.Split(',').Length > 5)
		{
			throw new Exception("The deviceIds input is limited to a maximum of 5 devices per call.");
		}

		if (configType != DeviceConfigsConfigType.Running && configType != DeviceConfigsConfigType.Startup && configType != DeviceConfigsConfigType.Both)
		{
			throw new Exception("The only valid input strings are RUNNING, STARTUP, and BOTH.");
		}

		var url = $"{restHttpClient.BaseAddress}pss/v1.0/inventory/customers/{customerId}/devices/{deviceIds}?configType={configType}";

		var response = await restHttpClient.GetAsync(url, cancellationToken);

		if (response.StatusCode == System.Net.HttpStatusCode.OK)
		{
			// Turns out the 

			/*
			// Filename is meant to be in the Content-Disposition header, but not always present.
			// Structure is like deviceconfig_100856745_996296262.zip
			var isZipped = response.Content.Headers.ContentDisposition?.FileName?.EndsWith("zip") == true;

			if (isZipped)
			{
			*/
			try
			{
				var memoryStream = new MemoryStream();
				await response.Content.CopyToAsync(memoryStream);
				memoryStream.Position = 0;

				if (memoryStream.Length == 0)
				{
					throw new Exception("The zip input stream is empty.");
				}

				return memoryStream;
			}
			catch (Exception ex)
			{
				throw new Exception("Unable to decompress the zipped response.", ex);
			}
			/*}

			throw new Exception("Response did not contain a zip file of configs.");
			*/
		}
		else
		{
			if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
			{
				throw new Exception($"None of the supplied device IDs have a config to return.");
			}

			throw new Exception($"An error occurred whilst requesting the config(s): {response.ReasonPhrase}");
		}
	}


	/// <inheritdoc/>
	public async Task<Dictionary<string, DeviceConfigResponse>> ExtractDeviceConfigsZipToDictionaryAsync(MemoryStream memoryStream)
	{
		// This method takes a MemoryStream and returns a Dictionary of DeviceConfigResponse objects.
		// If storing the result, consider compressing the properties first.

		Dictionary<string, DeviceConfigResponse> output = new();

		try
		{
			memoryStream.Position = 0; // Ensure the stream is at the beginning
			using (var zipInputStream = new ZipInputStream(memoryStream))
			{
				ZipEntry entry;

				while ((entry = zipInputStream.GetNextEntry()) != null)
				{
					/* Examples:
					 switches seem to be in this format:
					 499665469_2921733_PSS_2713922/1008264179_show running-config_2025_04_28.txt
					 whilst APs (and others?) are like this:
					 179888473_2921733_PSS_2713922/1008264180_show run-config_2025_04_28.txt
					 */

					var split = entry.Name.Split('/');
					if (split.Length != 2)
					{
						throw new Exception("Unable to parse the zipped response.");
					}

					var deviceId = split[1].Split('_').First();

					if (!output.ContainsKey(deviceId))
					{
						output[deviceId] = new DeviceConfigResponse();
					}

					using (var ms = new MemoryStream())
					{
						await zipInputStream.CopyToAsync(ms);
						ms.Position = 0;
						using (var sr = new StreamReader(ms))
						{
							var content = await sr.ReadToEndAsync();
							if (entry.Name.Contains("startup"))
							{
								output[deviceId].StartupConfig = content;
								var date = entry.Name.Split("config_").Last().Split('.').First();
								output[deviceId].StartupConfigDate = DateTime.ParseExact(date, "yyyy_MM_dd", null);
							}
							else if (entry.Name.Contains("running-config") || entry.Name.Contains("run-config"))
							{
								output[deviceId].RunningConfig = content;
								var date = entry.Name.Split("config_").Last().Split('.').First();
								output[deviceId].RunningConfigDate = DateTime.ParseExact(date, "yyyy_MM_dd", null);
							}
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception("Unable to decompress the zipped response.", ex);
		}

		return output;
	}

}