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

	/// <inheritdoc/>
	public async Task<Dictionary<string, DeviceConfigResponse>> GetDeviceConfigAsync(DeviceConfigsRequest deviceConfigsRequest, CancellationToken cancellationToken = default)
	{
		// This is a fairly new REST endpoint, unlike all the other PSS SOAP endpoints.
		// You can retrieve the running and startup configs for up to 5 devices at a time, in the form of a ZIP file.
		// Docs: https://docs.cloudapps.cisco.com/pss/APIDevGuide/service_inventory.html#getDeviceConfig

		// This method retrieves the zipped config file, extracts the contents, and returns them in a DeviceConfigResponse object.

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

		// All good

		var url = $"{restHttpClient.BaseAddress}pss/v1.0/inventory/customers/{customerId}/devices/{deviceIds}?configType={configType}";

		var response = restHttpClient.GetAsync(url, cancellationToken);

		//if (response.Result.IsSuccessStatusCode)
		if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
		{
			var isZipped = response.Result.Content.Headers.ContentDisposition?.FileName == "config.zip";

			if (isZipped)
			{
				// Populate the resonse with a record per deviceId
				Dictionary<string, DeviceConfigResponse> output = new();

				foreach(var deviceId in deviceConfigsRequest.DeviceIds) {
					output.Add(deviceId, new DeviceConfigResponse());
				}

				// Now process the zipped content
				try
				{
					// Expecting a ZIP file with one or two files per device ID - could be zero per deviceId if no config yet uploaded I presume.

					// Unzip response content using SharpZipLib, to get one or two files per device ID
					using (var zipInputStream = new ZipInputStream(response.Result.Content.ReadAsStreamAsync().Result))
					{
						ZipEntry entry;

						while ((entry = zipInputStream.GetNextEntry()) != null)
						{
							// 1922013981_2921733_PSS_2713922/1008264179_show running-config_2025_02_10.txt

							// Drop all characters upto and including the first /
							var split = entry.Name.Split('/');
							if (split.Length != 2)
							{
								throw new Exception("Unable to parse the zipped response.");
							}

							var deviceId = split[1].Split('_').First();

							using (var ms = new MemoryStream())
							{
								zipInputStream.CopyTo(ms);
								ms.Position = 0;
								using (var sr = new StreamReader(ms))
								{
									var content = sr.ReadToEnd();
									// Check if the file is a startup or running config
									if (entry.Name.Contains("startup"))
									{
										// Set the startup config
										output[deviceId].StartupConfig = content;

										// Get the date (e.g. 2024_07_31) from the end of the filename (excluding the extension)
										var date = entry.Name.Split("config_").Last().Split('.').First();
										output[deviceId].StartupConfigDate = DateTime.ParseExact(date, "yyyy_MM_dd", null);
									}
									else if (entry.Name.Contains("running"))
									{
										// Set the running config
										output[deviceId].RunningConfig = content;

										// Get the date from the filename
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
					throw new Exception("Unable to decompress the zipped response.");
				}

				return output;
			}

			// Wasn't zipped content
			throw new Exception("Response did not contain a zip file of configs.");
		}
		else
		{
			if (response.Result.StatusCode == System.Net.HttpStatusCode.NoContent)
			{
				throw new Exception($"SSH is disabled on this customer ID - configs cannot be retrieved.");
			}

			throw new Exception($"An error occurred whilst requesting the config(s): {response.Result.ReasonPhrase}");
		}
	}
}