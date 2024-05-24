using Cisco.Api.Data.PxCloud;
using Cisco.Api.Interfaces;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api;
internal class PxCloudReports : IPxCloudReports
{
	private HttpClient restHttpClient;

	private IPxCloudReportsInternal _refitClient;

	public PxCloudReports(HttpClient restHttpClient)
	{
		this.restHttpClient = restHttpClient;
		this._refitClient = RestService.For<IPxCloudReportsInternal>(restHttpClient);
	}

	/// <inheritdoc/>
	public async Task<RequestCustomerDataReportsAsBulkFilesResponse> RequestCustomerDataReportsAsBulkFilesAsync(string customerId, ReportName reportName, string successTrackId, CancellationToken cancellationToken = default)
	{
		// Normally, the required property is found within the 'location' header of the response, so extract the report ID from the end of the location
		var x = await _refitClient.RequestCustomerDataReportsAsBulkFilesAsync(customerId, reportName, successTrackId, cancellationToken).ConfigureAwait(false);

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
			throw new Exception("No location header found");
		}
	}

	/// <inheritdoc/>
	public async Task<ReportPayload> GetReportAsync(string customerId, string reportId, CancellationToken cancellationToken = default)
	{
		var url = $"{restHttpClient.BaseAddress}/px/v1/customers/{customerId}/reports/{reportId}";

		// Perform a Get("/px/v1/customers/{customerId}/reports/{reportId}") request to the REST API and store the response.
		// The response will normally be of type PxCloud.ReportResponse when deserialised. If this is the case, the property 'SuggestedNextPollTime' will tell us
		// when to poll the report again. If the response is instead a zipped file, then extract the file into a variable. Otherwise, throw an exception.

		while (true)
		{
			var response = restHttpClient.GetAsync(url, cancellationToken);
			if (response.Result.IsSuccessStatusCode)
			{
				// try to deserialize the response into reportresponse
				var reportResponse = JsonConvert.DeserializeObject<ReportResponse>(response.Result.Content.ToString() ?? "");
				if (reportResponse != null)
				{
					// wait for the suggested next poll time
					await Task.Delay(new TimeSpan(0, reportResponse.SuggestedNextPollTime, 0));
					// Resume loop;
					continue;
				}
				else
				{
					// Is the response a zipped file?
					var file = response.Result.Content.ReadAsStream();
					if (file != null)
					{
						// If so, extract the file into a variable
						var extractedZip = new System.IO.Compression.GZipStream(file, System.IO.Compression.CompressionMode.Decompress);
						if (extractedZip != null)
						{
							// Now that we have the contents, try to deserialize it into ReportPayload so we can check the report name
							var content = extractedZip.ToString() ?? "";
							var reportPayload = JsonConvert.DeserializeObject<ReportPayload>(content);
							if (reportPayload is null)
							{
								throw new Exception("Unable to deserialise the metadata for the requested report.");
							}

							var reportName = reportPayload.Metadata.ReportName;
							// (ReportName)Enum.Parse(typeof(ReportName), reportName);

							dynamic? b = reportName switch
							{
								"Assets" => JsonConvert.DeserializeObject<List<ReportPayloadItemsAssets>>(content) ?? throwError(reportName),
								"Software" => JsonConvert.DeserializeObject<List<ReportPayloadItemsSoftware>>(content) ?? throwError(reportName),
								"Hardware" => JsonConvert.DeserializeObject<List<ReportPayloadItemsHardware>>(content) ?? throwError(reportName),
								"FieldNotices" => JsonConvert.DeserializeObject<List<ReportPayloadItemsFieldNotices>>(content) ?? throwError(reportName),
								"ProrityBugs" => JsonConvert.DeserializeObject<List<ReportPayloadItemsPriorityBugs>>(content) ?? throwError(reportName),
								"SecurityAdvisories" => JsonConvert.DeserializeObject<List<ReportPayloadItemsSecurityAdvisories>>(content) ?? throwError(reportName),
								"PurchasedLicenses" => JsonConvert.DeserializeObject<List<ReportPayloadItemsPurchasedLicenses>>(content) ?? throwError(reportName),
								"Licenses" => JsonConvert.DeserializeObject<List<ReportPayloadItemsLicensesWithAssets>>(content) ?? throwError(reportName),
								_ => throw new NotSupportedException($"Unsupported report type: '{reportName}'."),
							};

							var finalReport = new ReportPayload()
							{
								Metadata = reportPayload.Metadata,
								Items = b
							};

							return finalReport;
						}
						else
						{
							throw new Exception("Response did not contain valid zipped file content.");
						}
					}
					else
					{
						throw new Exception("Response was neither a ReportResponse or zipped file content.");
					}
					// If so, extract the file into a variable
					// Otherwise, throw an exception


					throw new Exception("Error");
				}
				//return Task.FromResult(new ReportResponse());
			}
			else
			{
				throw new Exception("Error");
			}
		}
	}

	// Is fussy about the return type which can't just be null and must match the output of the deserialisation
	private dynamic throwError(string reportName)
	{
		throw new Exception($"An error occurred whilst deserialising the '{reportName}' report.");
	}



	//[Get("/px/v1/customers/{customerId}/reports/{reportId}")]
	//Task<ReportResponse> GetReportAsync(
	//	string customerId,
	//	string reportId,
	//	CancellationToken cancellationToken = default);

}