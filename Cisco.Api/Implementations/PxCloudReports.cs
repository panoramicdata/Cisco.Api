using Cisco.Api.Data.PxCloud;
using Cisco.Api.Exceptions;
using Cisco.Api.Interfaces;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using Refit;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Implementations;

internal class PxCloudReports(HttpClient restHttpClient) : IPxCloudReports
{
	private readonly IPxCloudReportsInternal _refitClient = RestService.For<IPxCloudReportsInternal>(restHttpClient);

	/// <inheritdoc/>
	public async Task<RequestCustomerDataReportsAsBulkFilesResponse> RequestCustomerDataReportAsync(string customerId, ReportName reportName, string successTrackId, CancellationToken cancellationToken = default)
	{
		// Normally, the required property is found within the 'location' header of the response, so extract the report ID from the end of the location
		var x = await _refitClient.RequestCustomerDataReportsAsBulkFilesAsync(
			customerId,
			new RequestCustomerDataReportsAsBulkFilesRequest
			{
				ReportName = reportName,
				SuccessTrackId = successTrackId
			}, cancellationToken).ConfigureAwait(false);

		if (x.Headers.TryGetValues("location", out var location))
		{
			return new RequestCustomerDataReportsAsBulkFilesResponse
			{
				// Take the report ID from the end of location
				ReportId = location.First().Split('/').Last()
			};
		}
		else
		{
			throw new PxCloudReportException("No location header found in response");
		}
	}

	/// <inheritdoc/>
	public async Task<ReportPayloadParent> GetReportAsync(string customerId, string reportId, CancellationToken cancellationToken = default)
	{
		var url = $"{restHttpClient.BaseAddress}/px/v1/customers/{customerId}/reports/{reportId}";

		while (true)
		{
			var response = restHttpClient.GetAsync(url, cancellationToken);
			if (!response.Result.IsSuccessStatusCode)
			{
				throw new PxCloudReportException("An error occurred whilst requesting the report.");
			}

			var isJson = response.Result.Content.Headers.ContentType?.MediaType == "application/json";
			var isZipped = response.Result.Content.Headers.ContentType?.MediaType == "application/zip";

			if (isJson)
			{
				await HandleJsonReportPollingAsync(response.Result, cancellationToken).ConfigureAwait(false);
				continue;
			}

			if (isZipped)
			{
				return await ExtractZippedReportAsync(response.Result, cancellationToken).ConfigureAwait(false);
			}

			throw new PxCloudReportException("Response did not contain a report status or final report content.");
		}
	}

	private async Task HandleJsonReportPollingAsync(HttpResponseMessage response, CancellationToken cancellationToken)
	{
		ReportResponse? reportResponse;
		try
		{
			var contentStream = response.Content.ReadAsStreamAsync(cancellationToken).Result;
			var content = new StreamReader(contentStream).ReadToEnd();
			reportResponse = JsonConvert.DeserializeObject<ReportResponse>(content ?? "");
		}
		catch (Exception ex)
		{
			throw new PxCloudReportException("Unable to deserialise JSON response for the requested report.", ex);
		}

		if (reportResponse is null)
		{
			throw new PxCloudReportException("Unable to deserialise JSON response for the requested report.");
		}

		await Task.Delay(new TimeSpan(0, 0, 15), cancellationToken);
	}

	private static async Task<ReportPayloadParent> ExtractZippedReportAsync(HttpResponseMessage response, CancellationToken cancellationToken)
	{
		string content = "";

		try
		{
			using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
			using var zipInputStream = new ZipInputStream(stream);
			ZipEntry entry;
			while ((entry = zipInputStream.GetNextEntry()) != null)
			{
				using var ms = new MemoryStream();
				zipInputStream.CopyTo(ms);
				ms.Position = 0;
				using var sr = new StreamReader(ms);
				content = sr.ReadToEnd();
			}
		}
		catch (Exception ex)
		{
			throw new PxCloudReportException("Unable to decompress the zipped response for the requested report.", ex);
		}

		if (content.Length == 0)
		{
			throw new PxCloudReportException("Response did not contain valid content.");
		}

		var reportPayload = JsonConvert.DeserializeObject<ReportPayloadParent>(content)
			?? throw new PxCloudReportException("Unable to deserialise the metadata for the requested report.");

		return DeserializeReportByName(reportPayload.Metadata.ReportName, content);
	}

	private static ReportPayloadParent DeserializeReportByName(string reportName, string content)
	{
		try
		{
			return reportName switch
			{
				"Assets" => JsonConvert.DeserializeObject<ReportPayloadParentAssets>(content) ?? ThrowError(reportName),
				"Software" => JsonConvert.DeserializeObject<ReportPayloadParentSoftware>(content) ?? ThrowError(reportName),
				"Hardware" => JsonConvert.DeserializeObject<ReportPayloadParentHardware>(content) ?? ThrowError(reportName),
				"FieldNotices" => JsonConvert.DeserializeObject<ReportPayloadParentFieldNotices>(content) ?? ThrowError(reportName),
				"Licenses" => JsonConvert.DeserializeObject<ReportPayloadParentLicensesWithAssets>(content) ?? ThrowError(reportName),
				"PurchasedLicenses" => JsonConvert.DeserializeObject<ReportPayloadParentPurchasedLicenses>(content) ?? ThrowError(reportName),
				"SecurityAdvisories" => JsonConvert.DeserializeObject<ReportPayloadParentSecurityAdvisories>(content) ?? ThrowError(reportName),
				"PriorityBugs" => JsonConvert.DeserializeObject<ReportPayloadParentPriorityBugs>(content) ?? ThrowError(reportName),
				_ => throw new NotSupportedException($"Unsupported report type: '{reportName}'."),
			};
		}
		catch (Exception ex) when (ex is not NotSupportedException and not PxCloudReportException)
		{
			throw new PxCloudReportException($"An error occurred whilst preparing the '{reportName}' report.", ex);
		}
	}

	// Is fussy about the return type which can't just be null and must match the output of the deserialisation
	private static dynamic ThrowError(string reportName)
		=> throw new PxCloudReportException($"An error occurred whilst deserialising the '{reportName}' report.");
}