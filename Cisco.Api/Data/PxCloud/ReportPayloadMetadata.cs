using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public class ReportPayloadMetadata
{
	/// <summary>
	/// The report name.
	/// </summary>
	[DataMember(Name = "reportName")]
	public required string ReportName { get; set; }

	/// <summary>
	/// The time the report was generated.
	/// </summary>
	[DataMember(Name = "reportRuntime")]
	public required string ReportRuntime { get; set; }

	/// <summary>
	/// The total count of items in the report.
	/// </summary>
	[DataMember(Name = "totalCount")]
	public required int TotalCount { get; set; }

	/// <summary>
	/// The user who requested the report.
	/// </summary>
	[DataMember(Name = "requestedBy")]
	public required string RequestedBy { get; set; }

	/// <summary>
	/// The customer ID.
	/// </summary>
	[DataMember(Name = "customerId")]
	public required string CustomerId { get; set; }
}
