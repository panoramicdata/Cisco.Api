using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;
public class RequestCustomerDataReportsAsBulkFilesResponse
{
	/// <summary>
	/// The estimated completion time.
	/// </summary>
	[DataMember(Name = "reportId")]
	public string ReportId { get; set; } = null!;

}
