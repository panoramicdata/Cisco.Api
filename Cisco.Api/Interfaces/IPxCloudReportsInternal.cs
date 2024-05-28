using Cisco.Api.Data.PxCloud;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Interfaces;
internal interface IPxCloudReportsInternal
{
	////////////////////////////////////////////////////////////////////////////
	/// Customer Insights and Analytics

	/// <summary>
	/// This API creates a request for customer data sets *and returns the response as ApiResponse<EmptyResponse>, just to get the Location header.*
	/// You can generate six kinds of reports: 'Assets', 'Software', 'Hardware', 'FieldNotices', 'ProrityBugs', 'SecurityAdvisories', 'PurchasedLicenses', and 'Licenses'.
	/// The response contains the reportID on the end of the Location header.
	/// </summary>
	/// <param name="customerId">Unique Identifier of the customer.</param>
	/// <param name="reportName">Name of the report that customer is looking for. It can be Assets, Software, Hardware, Field Notices, Priority bugs, and Security Advisories.</param>
	/// <param name="successTrackId">Unique identifier of the Success Track.</param>
	/// <param name="cancellationToken"></param>
	// TODO https://medium.com/net-core/using-refit-in-net-0843bb199987 Use this to get the response and return the reportId as json instead.
	[Post("/px/v1/customers/{customerId}/reports")]
	Task<ApiResponse<EmptyResponse>> RequestCustomerDataReportsAsBulkFilesAsync(
		string customerId,
		[Body] RequestCustomerDataReportsAsBulkFilesRequest requestCustomerDataReportsAsBulkFilesRequest,
		CancellationToken cancellationToken = default);

	// GetReportAsync() is too messy for Refit to deal with (the response can either be JSON or a Zip), so handled in PxCloudNonRefit.cs
}
