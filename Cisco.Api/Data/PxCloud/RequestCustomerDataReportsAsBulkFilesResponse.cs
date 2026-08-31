using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;
/// <summary>
/// Represents the request customer data reports as bulk files response.
/// </summary>
public class RequestCustomerDataReportsAsBulkFilesResponse
{
	/// <summary>
	/// The estimated completion time.
	/// </summary>
	[DataMember(Name = "reportId")]
	public string ReportId { get; set; } = null!;

}
