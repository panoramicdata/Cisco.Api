using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api
{
	[DataContract]
	public class CoverageStatusCollection
	{
		[DataMember(Name = "serial_numbers")]
		public List<CoverageStatus> CoverageStatuses { get; set; }
	}
}