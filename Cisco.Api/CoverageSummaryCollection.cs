using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api
{
	[DataContract]
	public class CoverageSummaryCollection
	{
		[DataMember(Name = "serial_numbers")]
		public List<CoverageSummary> CoverageSummaries { get; set; }
	}
}