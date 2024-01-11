using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SerialNumberToInfo;

/// <summary>
/// A coverage summary collection
/// </summary>
[DataContract]
public class CoverageSummaryCollection
{
	/// <summary>
	/// The coverage summaries
	/// </summary>
	[DataMember(Name = "serial_numbers")]
	public List<CoverageSummary> CoverageSummaries { get; set; } = null!;
}