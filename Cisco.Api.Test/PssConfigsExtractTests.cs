using Cisco.Api.Data.Pss;
using Cisco.Api.Implementations;
using Cisco.Api.Exceptions;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Offline tests for <see cref="PssConfigs"/>. The handler is a stub: no network, no credentials.
/// </summary>
public class PssConfigsExtractTests
{
	private readonly PssConfigs _sut = new(new HttpClient(new StubHandler(HttpStatusCode.OK, "{}"))
	{
		BaseAddress = new Uri("https://api.cisco.com/")
	});

	/// <summary>Empty device list is rejected up front, before any HTTP call.</summary>
	[Fact]
	public async Task RetrieveDeviceConfigZipAsync_NoDeviceIds_Throws()
	{
		await Assert.ThrowsAsync<PssConfigException>(
			() => _sut.RetrieveDeviceConfigZipAsync(new DeviceConfigsRequest
			{
				CustomerId = "123",
				DeviceIds = [],
				ConfigType = DeviceConfigsConfigType.Running
			}));
	}

	/// <summary>More than five devices in one call is rejected.</summary>
	[Fact]
	public async Task RetrieveDeviceConfigZipAsync_MoreThanFiveDevices_Throws()
	{
		var request = new DeviceConfigsRequest
		{
			CustomerId = "123",
			DeviceIds = ["a", "b", "c", "d", "e", "f"],
			ConfigType = DeviceConfigsConfigType.Running
		};
		await Assert.ThrowsAsync<PssConfigException>(() => _sut.RetrieveDeviceConfigZipAsync(request));
	}

	/// <summary>A 204 from the API means no device had a config to return.</summary>
	[Fact]
	public async Task RetrieveDeviceConfigZipAsync_NoContent_Throws()
	{
		var sut = new PssConfigs(new HttpClient(new StubHandler(HttpStatusCode.NoContent, ""))
		{
			BaseAddress = new Uri("https://api.cisco.com/")
		});
		var request = new DeviceConfigsRequest
		{
			CustomerId = "123",
			DeviceIds = ["a"],
			ConfigType = DeviceConfigsConfigType.Both
		};
		await Assert.ThrowsAsync<PssConfigException>(() => sut.RetrieveDeviceConfigZipAsync(request));
	}

	/// <summary>A non-OK status raises a PssConfigException carrying the reason phrase.</summary>
	[Fact]
	public async Task RetrieveDeviceConfigZipAsync_BadStatus_Throws()
	{
		var sut = new PssConfigs(new HttpClient(new StubHandler(HttpStatusCode.InternalServerError, "oops"))
		{
			BaseAddress = new Uri("https://api.cisco.com/")
		});
		var request = new DeviceConfigsRequest
		{
			CustomerId = "123",
			DeviceIds = ["a"],
			ConfigType = DeviceConfigsConfigType.Startup
		};
		var ex = await Assert.ThrowsAsync<PssConfigException>(() => sut.RetrieveDeviceConfigZipAsync(request));
		ex.Message.Should().Contain("error occurred");
	}

	/// <summary>An empty body is reported with a dedicated message.</summary>
	[Fact]
	public async Task RetrieveDeviceConfigZipAsync_EmptyBody_Throws()
	{
		var sut = new PssConfigs(new HttpClient(new StubHandler(HttpStatusCode.OK, ""))
		{
			BaseAddress = new Uri("https://api.cisco.com/")
		});
		var request = new DeviceConfigsRequest
		{
			CustomerId = "123",
			DeviceIds = ["a"],
			ConfigType = DeviceConfigsConfigType.Both
		};
		var ex = await Assert.ThrowsAsync<PssConfigException>(() => sut.RetrieveDeviceConfigZipAsync(request));
		ex.Message.Should().Contain("empty");
	}

	/// <summary>The right config slot is populated depending on the entry name.</summary>
	[Theory]
	[InlineData("c1/d1_show startup-config_2025_04_28.txt", "d1", "startup")]
	[InlineData("c1/d1_show running-config_2025_04_28.txt", "d1", "running")]
	[InlineData("c1/d1_show run-config_2025_04_28.txt", "d1", "running")]
	public async Task ExtractDeviceConfigsZipToDictionaryAsync_SetsTheExpectedConfig(string entryName, string deviceId, string which)
	{
		using var stream = BuildZip(entryName, "config-content");

		var result = await _sut.ExtractDeviceConfigsZipToDictionaryAsync(stream);

		result.Should().ContainKey(deviceId);
		if (which == "startup")
		{
			result[deviceId].StartupConfig.Should().Be("config-content");
			result[deviceId].RunningConfig.Should().BeNull();
			result[deviceId].StartupConfigDate.Should().Be(new DateTime(2025, 4, 28));
		}
		else
		{
			result[deviceId].RunningConfig.Should().Be("config-content");
			result[deviceId].StartupConfig.Should().BeNull();
			result[deviceId].RunningConfigDate.Should().Be(new DateTime(2025, 4, 28));
		}
	}

	/// <summary>An entry name that cannot be split on '/' is rejected.</summary>
	[Fact]
	public async Task ExtractDeviceConfigsZipToDictionaryAsync_UnparseableEntryName_Throws()
	{
		using var stream = BuildZip("onlyonepart", "content");
		await Assert.ThrowsAsync<PssConfigException>(() => _sut.ExtractDeviceConfigsZipToDictionaryAsync(stream));
	}

	/// <summary>When the entry name has no parsable date, the date stays null.</summary>
	[Fact]
	public async Task ExtractDeviceConfigsZipToDictionaryAsync_NoDateInName_LeavesDateNull()
	{
		using var stream = BuildZip("c1/d1_show startup-config_nodate.txt", "config-content");
		var result = await _sut.ExtractDeviceConfigsZipToDictionaryAsync(stream);
		result["d1"].StartupConfig.Should().Be("config-content");
		result["d1"].StartupConfigDate.Should().BeNull();
	}

	/// <summary>A stream that is not a zip raises a PssConfigException.</summary>
	[Fact]
	public async Task ExtractDeviceConfigsZipToDictionaryAsync_MalformedZip_Throws()
	{
		using var stream = new MemoryStream(new byte[] { 1, 2, 3, 4 });
		await Assert.ThrowsAsync<PssConfigException>(() => _sut.ExtractDeviceConfigsZipToDictionaryAsync(stream));
	}

	private static MemoryStream BuildZip(string entryName, string content)
	{
		var memoryStream = new MemoryStream();
		using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
		{
			var entry = archive.CreateEntry(entryName);
			using var writer = new StreamWriter(entry.Open());
			writer.Write(content);
		}
		memoryStream.Position = 0;
		return memoryStream;
	}

	private sealed class StubHandler(HttpStatusCode status, string body) : HttpMessageHandler
	{
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
			=> Task.FromResult(new HttpResponseMessage(status) { Content = new StringContent(body) });
	}
}
