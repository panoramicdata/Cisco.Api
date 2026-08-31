using Cisco.Api.Data.Pss;
using Cisco.Api.Exceptions;
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
internal class PssConfigs(HttpClient restHttpClient) : IPssConfigs
{
	public async Task<MemoryStream> RetrieveDeviceConfigZipAsync(DeviceConfigsRequest deviceConfigsRequest, CancellationToken cancellationToken = default)
	{
		ValidateDeviceConfigsRequest(deviceConfigsRequest);

		var customerId = deviceConfigsRequest.CustomerId;
		var deviceIds = string.Join(",", deviceConfigsRequest.DeviceIds);
		var configType = deviceConfigsRequest.ConfigType;
		var url = $"{restHttpClient.BaseAddress}pss/v1.0/inventory/customers/{customerId}/devices/{deviceIds}?configType={configType}";

		var response = await restHttpClient.GetAsync(url, cancellationToken);

		if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
		{
			throw new PssConfigException("None of the supplied device IDs have a config to return.");
		}

		if (response.StatusCode != System.Net.HttpStatusCode.OK)
		{
			throw new PssConfigException($"An error occurred whilst requesting the config(s): {response.ReasonPhrase}");
		}

		try
		{
			var memoryStream = new MemoryStream();
			await response.Content.CopyToAsync(memoryStream, cancellationToken);
			memoryStream.Position = 0;

			if (memoryStream.Length == 0)
			{
				throw new PssConfigException("The zip input stream is empty.");
			}

			return memoryStream;
		}
		catch (Exception ex) when (ex is not PssConfigException)
		{
			throw new PssConfigException("Unable to decompress the zipped response.", ex);
		}
	}

	private static void ValidateDeviceConfigsRequest(DeviceConfigsRequest deviceConfigsRequest)
	{
		if (deviceConfigsRequest.DeviceIds.Count == 0)
		{
			throw new PssConfigException("No device IDs provided.");
		}

		if (deviceConfigsRequest.DeviceIds.Count > 5)
		{
			throw new PssConfigException("The deviceIds input is limited to a maximum of 5 devices per call.");
		}

		var configType = deviceConfigsRequest.ConfigType;
		if (configType != DeviceConfigsConfigType.Running && configType != DeviceConfigsConfigType.Startup && configType != DeviceConfigsConfigType.Both)
		{
			throw new PssConfigException("The only valid input strings are RUNNING, STARTUP, and BOTH.");
		}
	}


	/// <inheritdoc/>
	public async Task<Dictionary<string, DeviceConfigResponse>> ExtractDeviceConfigsZipToDictionaryAsync(MemoryStream memoryStream)
	{
		// This method takes a MemoryStream and returns a Dictionary of DeviceConfigResponse objects.
		// If storing the result, consider compressing the properties first.

		Dictionary<string, DeviceConfigResponse> output = [];

		try
		{
			memoryStream.Position = 0; // Ensure the stream is at the beginning
			using var zipInputStream = new ZipInputStream(memoryStream);
			while (zipInputStream.GetNextEntry() is { } entry)
			{
				await AddEntryAsync(zipInputStream, entry, output).ConfigureAwait(false);
			}
		}
		catch (Exception ex) when (ex is not PssConfigException)
		{
			throw new PssConfigException("Unable to decompress the zipped response.", ex);
		}

		return output;
	}

	private static async Task AddEntryAsync(
		ZipInputStream zipInputStream,
		ZipEntry entry,
		Dictionary<string, DeviceConfigResponse> output)
	{
		var deviceId = GetDeviceId(entry.Name);
		if (!output.TryGetValue(deviceId, out var deviceConfig))
		{
			deviceConfig = new DeviceConfigResponse();
			output[deviceId] = deviceConfig;
		}

		using var memoryStream = new MemoryStream();
		await zipInputStream.CopyToAsync(memoryStream).ConfigureAwait(false);
		memoryStream.Position = 0;
		using var reader = new StreamReader(memoryStream);
		var content = await reader.ReadToEndAsync().ConfigureAwait(false);
		SetConfig(entry.Name, content, deviceConfig);
	}

	private static string GetDeviceId(string entryName)
	{
		/* Examples:
		 switches seem to be in this format:
		 499665469_2921733_PSS_2713922/1008264179_show running-config_2025_04_28.txt
		 whilst APs (and others?) are like this:
		 179888473_2921733_PSS_2713922/1008264180_show run-config_2025_04_28.txt
		 */
		var split = entryName.Split('/');
		if (split.Length != 2)
		{
			throw new PssConfigException("Unable to parse the zipped response.");
		}

		return split[1].Split('_').First();
	}

	private static void SetConfig(string entryName, string content, DeviceConfigResponse deviceConfig)
	{
		if (entryName.Contains("startup", StringComparison.Ordinal))
		{
			deviceConfig.StartupConfig = content;
			deviceConfig.StartupConfigDate = TryExtractDateFromEntryName(entryName);
		}
		else if (entryName.Contains("running-config", StringComparison.Ordinal)
			|| entryName.Contains("run-config", StringComparison.Ordinal))
		{
			deviceConfig.RunningConfig = content;
			deviceConfig.RunningConfigDate = TryExtractDateFromEntryName(entryName);
		}
	}

	/// <summary>
	/// Attempts to extract a date from a zip entry name.
	/// Expected format: ...config_yyyy_MM_dd.txt
	/// Returns null if date cannot be parsed.
	/// </summary>
	private static DateTime? TryExtractDateFromEntryName(string entryName)
	{
		// Expected format: ...config_yyyy_MM_dd.txt
		var configSplit = entryName.Split("config_");
		if (configSplit.Length < 2)
		{
			return null;
		}

		var datePart = configSplit.Last().Split('.').First();
		if (DateTime.TryParseExact(datePart, "yyyy_MM_dd", null, System.Globalization.DateTimeStyles.None, out var parsedDate))
		{
			return parsedDate;
		}

		return null;
	}

}
