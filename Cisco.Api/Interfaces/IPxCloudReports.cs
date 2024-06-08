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
	/// You can request the following customer reports: 'Assets', 'Software', 'Hardware', 'FieldNotices', 'SecurityAdvisories', 'PurchasedLicenses', and 'Licenses'.
	/// The response contains the scheduled reportID.
	/// </summary>
	/// <param name="customerId">Unique Identifier of the customer.</param>
	/// <param name="reportName">Name of the report that customer is looking for.</param>
	/// <param name="successTrackId">Unique identifier of the Success Track.</param>
	/// <param name="cancellationToken"></param>
	[Post("/px/v1/customers/{customerId}/reports")]
	Task<RequestCustomerDataReportsAsBulkFilesResponse> RequestCustomerDataReportAsync(
		string customerId,
		ReportName reportName,
		string successTrackId,
		CancellationToken cancellationToken = default);

	/// Returns the scheduled report content; this may take a few minutes to generate.
	/// <param name="customerId">Unique Identifier of the customer.</param>
	/// <param name="reportId">Report ID.</param>
	/// <param name="cancellationToken"></param>
	[Get("/px/v1/customers/{customerId}/reports/{reportId}")]
	Task<ReportPayloadParent> GetReportAsync(
		string customerId,
		string reportId,
		CancellationToken cancellationToken = default);
}
