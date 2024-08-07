﻿using Cisco.Api.Data.Pss;
using Cisco.Api.Interfaces;
using ICSharpCode.SharpZipLib.Zip;
using System;
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
	public async Task<DeviceConfigResponse> GetDeviceConfigAsync(DeviceConfigsRequest deviceConfigsRequest, CancellationToken cancellationToken = default)
	{
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

		if (configType != DeviceConfigsConfigType.RunningConfig && configType != DeviceConfigsConfigType.StartupConfig && configType != DeviceConfigsConfigType.Both)
		{
			throw new Exception("The only valid input strings are RUNNING, STARTUP, and BOTH.");
		}

		// All good

		var url = $"{restHttpClient.BaseAddress}pss/v1.0/inventory/customers/{customerId}/devices/{deviceIds}?configType={configType}";

		// Perform a Get("/px/v1/customers/{customerId}/reports/{reportId}") request to the REST API.
		// The response is hopefully a zipped file, which should contain one or both of the following: 'startup' and 'running' configurations.
		// An example if "126965383_2921733_PSS_240936/636053762_show startup-config_2024_07_31.txt"
		// An example if "126965383_2921733_PSS_240936/636053762_show running-config_2024_07_31.txt"
		// Extract the files based on whether the word 'startup' or 'running' into properties. Also set the related date property.
		// If the response is not a zipped file, then throw an exception.

		var response = restHttpClient.GetAsync(url, cancellationToken);
		if (response.Result.IsSuccessStatusCode)
		{
			var isZipped = response.Result.Content.Headers.ContentDisposition?.FileName == "config.zip";

			if (isZipped)
			{
				DeviceConfigResponse output = new();

				try
				{
					// Unzip response content using SharpZipLib, to get one or two files
					using (var zipInputStream = new ZipInputStream(response.Result.Content.ReadAsStreamAsync().Result))
					{
						ZipEntry entry;
						while ((entry = zipInputStream.GetNextEntry()) != null)
						{
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
										output.StartupConfig = content;

										// Get the date (e.g. 2024_07_31) from the end of the filename (excluding the extension)
										var date = entry.Name.Split("config_").Last().Split('.').First();
										output.StartupConfigDate = DateTime.ParseExact(date, "yyyy_MM_dd", null);
									}
									else if (entry.Name.Contains("running"))
									{
										// Set the running config
										output.RunningConfig = content;

										// Get the date from the filename
										var date = entry.Name.Split("config_").Last().Split('.').First();
										output.RunningConfigDate = DateTime.ParseExact(date, "yyyy_MM_dd", null);
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
			throw new Exception("Response did not contain any configs.");
		}
		else
		{
			throw new Exception("An error occurred whilst requesting the config(s).");
		}
	}
}