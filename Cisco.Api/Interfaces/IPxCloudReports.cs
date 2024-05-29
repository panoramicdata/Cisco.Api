﻿using Cisco.Api.Data.PxCloud;
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
	/// You can request the following reports: 'Assets', 'Software', 'Hardware', 'FieldNotices', and 'SecurityAdvisories'.
	/// The following will be implemented soon: 'PriorityBugs', 'PurchasedLicenses', and 'Licenses'.
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

	/// Waits until the scheduled report is available, then returns the content.
	/// <param name="customerId">Unique Identifier of the customer.</param>
	/// <param name="reportId">Report ID.</param>
	/// <param name="cancellationToken"></param>
	[Get("/px/v1/customers/{customerId}/reports/{reportId}")]
	Task<ReportPayloadParent> GetReportAsync(
		string customerId,
		string reportId,
		CancellationToken cancellationToken = default);
}
