using Cisco.Api.Data.PxCloud;
using Cisco.Api.Interfaces;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using Refit;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Implementations;
internal class PxCloudReports : IPxCloudReports
{
	private HttpClient restHttpClient;

	private IPxCloudReportsInternal _refitClient;

	public PxCloudReports(HttpClient restHttpClient)
	{
		this.restHttpClient = restHttpClient;
		_refitClient = RestService.For<IPxCloudReportsInternal>(restHttpClient);
	}

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
			throw new Exception("No location header found");
		}
	}

	/// <inheritdoc/>
	public async Task<ReportPayloadParent> GetReportAsync(string customerId, string reportId, CancellationToken cancellationToken = default)
	{
		var url = $"{restHttpClient.BaseAddress}/px/v1/customers/{customerId}/reports/{reportId}";

		// Perform a Get("/px/v1/customers/{customerId}/reports/{reportId}") request to the REST API.
		// The response will typically be JSON of type PxCloud.ReportResponse. If this is the case, the property 'suggestedNextPollTimeInMins' will
		// tell us when to poll the report again but it is way off, so just try again after X seconds. If the response is instead a zipped file,
		// then extract the file just to get the Metadata - that report name will tell us what type of report it is, and then we can deserialise the
		// extracted zip content from scratch so that Items can have the correct type.
		// If the response is neither JSON nor a zipped file, then throw an exception.

		while (true)
		{
			var response = restHttpClient.GetAsync(url, cancellationToken);
			if (response.Result.IsSuccessStatusCode)
			{
				var isJson = response.Result.Content.Headers.ContentType?.MediaType == "application/json";
				var isZipped = response.Result.Content.Headers.ContentType?.MediaType == "application/zip";

				if (isJson)
				{
					ReportResponse? reportResponse;
					try
					{
						// Extract the content from the response
						var contentStream = response.Result.Content.ReadAsStreamAsync().Result;
						var content = new StreamReader(contentStream).ReadToEnd();
						// Examples:
						// {"status":"Accepted"}
						// {"status":"In Progress","suggestedNextPollTimeInMins":2,"suggestedNextPollInterval":2}

						// try to deserialize the response into reportResponse
						reportResponse = JsonConvert.DeserializeObject<ReportResponse>(content ?? "");
					}
					catch (Exception ex)
					{
						throw new Exception("Unable to deserialise JSON response for the requested report.");
					}
					if (reportResponse is null)
					{
						throw new Exception("Unable to deserialise JSON response for the requested report.");
					}

					// wait for the suggested next poll time
					// await Task.Delay(new TimeSpan(0, reportResponse.SuggestedNextPollTimeInMins, 0));

					// Ignore the interval, repoll in X seconds (above might return 2 mins, but data was ready in under 30 seconds)
					await Task.Delay(new TimeSpan(0, 0, 15));

					// Resume loop;
					continue;
				}

				if (isZipped)
				{
					string content = "";

					try
					{
						// Unzip response content using SharpZipLib
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
										content = sr.ReadToEnd();
									}
								}
							}
						}

					}
					catch (Exception ex)
					{
						throw new Exception("Unable to decompress the zipped response for the requested report.");
					}

					if (content.Length > 0)
					{
						// Now that we have the contents, try to deserialize it into ReportPayload so we can check the report name

						var reportPayload = JsonConvert.DeserializeObject<ReportPayloadParent>(content);
						if (reportPayload is null)
						{
							throw new Exception("Unable to deserialise the metadata for the requested report.");
						}

						var reportName = reportPayload.Metadata.ReportName;
						try
						{
							dynamic? populatedItems = null!;

							populatedItems = reportName switch
							{
								// Note: This works, but I think this could be more generic and not need each of the ReportPayloadParent classes.

								"Assets" => JsonConvert.DeserializeObject<ReportPayloadParentAssets>(content) ?? throwError(reportName),
								"Software" => JsonConvert.DeserializeObject<ReportPayloadParentSoftware>(content) ?? throwError(reportName),
								"Hardware" => JsonConvert.DeserializeObject<ReportPayloadParentHardware>(content) ?? throwError(reportName),
								"FieldNotices" => JsonConvert.DeserializeObject<ReportPayloadParentFieldNotices>(content) ?? throwError(reportName),
								"Licenses" => JsonConvert.DeserializeObject<ReportPayloadParentLicensesWithAssets>(content) ?? throwError(reportName),
								"PurchasedLicenses" => JsonConvert.DeserializeObject<ReportPayloadParentPurchasedLicenses>(content) ?? throwError(reportName),
								"SecurityAdvisories" => JsonConvert.DeserializeObject<ReportPayloadParentSecurityAdvisories>(content) ?? throwError(reportName),

								// Not seen/implemented yet as there's no API documentation
								//"PriorityBugs" => JsonConvert.DeserializeObject<ReportPayloadParentPriorityBugs>(content) ?? throwError(reportName),

								_ => throw new NotSupportedException($"Unsupported report type: '{reportName}'."),
							};

							return populatedItems;

						}
						catch (Exception ex)
						{
							throw new Exception($"An error occurred whilst preparing the '{reportName}' report.");
						}
					}
					else
					{
						throw new Exception("Response did not contain valid content.");
					}
				}

				// Wasn't JSON or zipped report content
				throw new Exception("Response did not contain a report status or final report content.");
			}
			else
			{
				throw new Exception("An error occurred whilst requesting the report.");
			}
		}
	}

	// Is fussy about the return type which can't just be null and must match the output of the deserialisation
	private dynamic throwError(string reportName)
	{
		throw new Exception($"An error occurred whilst deserialising the '{reportName}' report.");
	}

	public async static Task<string> DecompressAsync(string value)
	{
		//byte[] buffer = Encoding.UTF8.GetBytes(value);
		byte[] buffer = Convert.FromBase64String(value);
		byte[] decompressed;

		using (var inputStream = new MemoryStream(buffer))
		{
			using var outputStream = new MemoryStream();

			using (var brotliStream = new GZipStream(inputStream, CompressionMode.Decompress, leaveOpen: true))
			{
				await brotliStream.CopyToAsync(outputStream);
			}

			decompressed = outputStream.ToArray();
		}

		return Encoding.UTF8.GetString(decompressed);
	}

	//[Get("/px/v1/customers/{customerId}/reports/{reportId}")]
	//Task<ReportResponse> GetReportAsync(
	//	string customerId,
	//	string reportId,
	//	CancellationToken cancellationToken = default);

}