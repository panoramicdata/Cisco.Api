using Cisco.Api.Data.PxCloud;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Interfaces;
public interface IPxCloudReports
{
	////////////////////////////////////////////////////////////////////////////
	/// Customer Insights and Analytics

	/// <summary>
	/// This API creates request for customer data sets.
	/// You can generate six kinds of reports: 'Assets', 'Software', 'Hardware', 'FieldNotices', 'ProrityBugs', 'SecurityAdvisories', 'PurchasedLicenses', and 'Licenses'.
	/// The response contains the scheduled reportID.
	/// </summary>
	/// <param name="customerId">Unique Identifier of the customer.</param>
	/// <param name="reportName">Name of the report that customer is looking for. It can be Assets, Software, Hardware, Field Notices, Priority bugs, and Security Advisories.</param>
	/// <param name="successTrackId">Unique identifier of the Success Track.</param>
	/// <param name="cancellationToken"></param>
	// TODO https://medium.com/net-core/using-refit-in-net-0843bb199987 Use this to get the response and return the reportId as json instead.
	[Post("/px/v1/customers/{customerId}/reports")]
	Task<RequestCustomerDataReportsAsBulkFilesResponse> RequestCustomerDataReportAsync(
		string customerId,
		ReportName reportName,
		string successTrackId,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Provides the status of resource. The status API provides additional info about the progress of the report.
	/// Partner application should poll periodically to get status of the report.
	/// When the report is complete the API responds with the 303 status pointing to final report in the Location header.
	/// </summary>
	/// <param name="customerId">Unique Identifier of the customer.</param>
	/// <param name="reportId">Report ID.</param>
	/// <param name="cancellationToken"></param>
	[Get("/px/v1/customers/{customerId}/reports/{reportId}")]
	Task<ReportPayloadParentWithoutItems> GetReportAsync(
		string customerId,
		string reportId,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Schedule a report, wait for completion and download the report.
	/// </summary>
	/// <param name="customerId">Unique Identifier of the customer.</param>
	/// <param name="reportName">Report type. Valid values : 'Assets', 'FieldNotices', 'Hardware', 'Licenses', 'PurchasedLicenses', 'SecurityAdvisories', 'Software', 'PriorityBugs'.</param>
	/// <param name="successTrackId">Unique identifier of the Success Track.</param>
	/// <param name="cancellationToken"></param>
	//public async Task GenerateReportAsync(
	//	string customerId,
	//	ReportName reportName,
	//	string successTrackId,
	//	CancellationToken cancellationToken = default)
	//{

	//	// Call 'RequestCustomerDataReportsAsBulkFilesAsync' to request the report, and store the query headers.
	//	// The response contains the reportID on the end of the Location header.
	//	var response = await RequestCustomerDataReportsAsBulkFilesAsync(customerId, reportName, successTrackId, cancellationToken).ConfigureAwait(false);

	//}
}
