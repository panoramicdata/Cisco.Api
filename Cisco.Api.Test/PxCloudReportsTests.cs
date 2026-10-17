using Cisco.Api.Data.PxCloud;
using Cisco.Api.Implementations;
using Cisco.Api.Exceptions;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Offline tests for <see cref="PxCloudReports"/>. The handler is a stub; no network or credentials.
/// </summary>
public class PxCloudReportsTests
{
	/// <summary>The ReportId is taken from the last segment of the Location header.</summary>
	[Fact]
	public async Task RequestCustomerDataReportAsync_ReadsTheReportIdFromTheLocationHeader()
	{
		var handler = new StubHandler(HttpStatusCode.OK, Array.Empty<byte>()) { Location = "https://api.cisco.com/reports/abc-123" };
		var sut = Build(handler);
		var result = await sut.RequestCustomerDataReportAsync("123", ReportName.Assets, "ST123", CancellationToken.None);
		result.ReportId.Should().Be("abc-123");
	}

	/// <summary>A failed request raises a PxCloudReportException.</summary>
	[Fact]
	public async Task RequestCustomerDataReportAsync_FailedRequest_Throws()
	{
		// A non-2xx response through Refit's IApiResponse wrapper surfaces the framework's own
		// ApiException rather than letting the code's own check run. Accept either exception.
		var sut = Build(new StubHandler(HttpStatusCode.BadRequest, Array.Empty<byte>()));
		var ex = await Assert.ThrowsAnyAsync<Exception>(
			() => sut.RequestCustomerDataReportAsync("123", ReportName.Assets, "ST123", CancellationToken.None));
		_ = ex;
	}

	/// <summary>An OK response without a Location header raises a PxCloudReportException.</summary>
	[Fact]
	public async Task RequestCustomerDataReportAsync_MissingLocationHeader_Throws()
	{
		var sut = Build(new StubHandler(HttpStatusCode.OK, Array.Empty<byte>()));
		await Assert.ThrowsAsync<PxCloudReportException>(
			() => sut.RequestCustomerDataReportAsync("123", ReportName.Assets, "ST123", CancellationToken.None));
	}

	/// <summary>Every supported report name deserialises into the right payload type.</summary>
	[Theory]
	[InlineData("Assets")]
	[InlineData("FieldNotices")]
	[InlineData("Hardware")]
	[InlineData("Licenses")]
	[InlineData("PurchasedLicenses")]
	[InlineData("SecurityAdvisories")]
	[InlineData("Software")]
	[InlineData("PriorityBugs")]
	public async Task GetReportAsync_ZippedReport_DeserialisesEverySupportedType(string reportName)
	{
		var handler = new StubHandler(HttpStatusCode.OK, BuildZipBytes(reportName))
		{
			ContentType = "application/zip"
		};
		var sut = Build(handler);
		var result = await sut.GetReportAsync("123", "abc", CancellationToken.None);
		result.Metadata.ReportName.Should().Be(reportName);
	}

	/// <summary>An unsupported report name is rejected with a NotSupportedException.</summary>
	[Fact]
	public async Task GetReportAsync_UnsupportedReportName_Throws()
	{
		var handler = new StubHandler(HttpStatusCode.OK, BuildZipBytes("Bogus")) { ContentType = "application/zip" };
		var sut = Build(handler);
		await Assert.ThrowsAsync<NotSupportedException>(
			() => sut.GetReportAsync("123", "abc", CancellationToken.None));
	}

	/// <summary>A non-OK response raises a PxCloudReportException.</summary>
	[Fact]
	public async Task GetReportAsync_FailedRequest_Throws()
	{
		var sut = Build(new StubHandler(HttpStatusCode.InternalServerError, Array.Empty<byte>()));
		await Assert.ThrowsAsync<PxCloudReportException>(
			() => sut.GetReportAsync("123", "abc", CancellationToken.None));
	}

	/// <summary>A response with an unrecognised content type raises a PxCloudReportException.</summary>
	[Fact]
	public async Task GetReportAsync_UnknownContentType_Throws()
	{
		var sut = Build(new StubHandler(HttpStatusCode.OK, new byte[] { 1, 2, 3 }) { ContentType = "text/html" });
		await Assert.ThrowsAsync<PxCloudReportException>(
			() => sut.GetReportAsync("123", "abc", CancellationToken.None));
	}

	/// <summary>A malformed zip is reported through a PxCloudReportException.</summary>
	[Fact]
	public async Task GetReportAsync_MalformedZip_Throws()
	{
		var handler = new StubHandler(HttpStatusCode.OK, new byte[] { 1, 2, 3, 4 }) { ContentType = "application/zip" };
		var sut = Build(handler);
		await Assert.ThrowsAsync<PxCloudReportException>(
			() => sut.GetReportAsync("123", "abc", CancellationToken.None));
	}

	private static PxCloudReports Build(StubHandler handler)
		=> new(new HttpClient(handler)
		{
			BaseAddress = new Uri("https://api.cisco.com/")
		});

	private static byte[] BuildZipBytes(string reportName)
	{
		var json = $$"""{"metadata":{"reportName":"{{reportName}}","reportRuntime":"1s","totalCount":0,"requestedBy":"me","customerId":"123"},"items":[]}""";
		var memoryStream = new MemoryStream();
		using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
		{
			var entry = archive.CreateEntry("payload.json");
			using var writer = new StreamWriter(entry.Open(), Encoding.UTF8);
			writer.Write(json);
		}
		return memoryStream.ToArray();
	}

	private sealed class StubHandler(HttpStatusCode status, byte[] body) : HttpMessageHandler
	{
		private readonly HttpStatusCode _status = status;
		private readonly byte[] _body = body;
		public string? Location { get; set; }
		public string ContentType { get; set; } = "application/octet-stream";

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			var response = new HttpResponseMessage(_status)
			{
				Content = new ByteArrayContent(_body)
			};
			response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ContentType);
			if (Location is not null)
			{
				response.Headers.TryAddWithoutValidation("location", Location);
			}
			return Task.FromResult(response);
		}
	}
}
