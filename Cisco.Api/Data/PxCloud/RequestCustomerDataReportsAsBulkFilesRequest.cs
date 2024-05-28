using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;
internal class RequestCustomerDataReportsAsBulkFilesRequest
{
	/// <summary>
	/// The report name.
	/// </summary>
	[DataMember(Name = "reportName")]
	public ReportName ReportName { get; set; }

	/// <summary>
	/// The success track ID.
	/// </summary>
	[DataMember(Name = "successTrackId")]
	public string SuccessTrackId { get; set; } = null!;
}
