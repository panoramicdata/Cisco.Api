using System.Runtime.Serialization;

namespace Cisco.Api
{
	[DataContract]
	public class CoverageStatus
	{
		[DataMember(Name = "sr_no")]
		public string SerialNumber { get; set; }

		[DataMember(Name = "is_covered")]
		public string IsCovered { get; set; }

		[DataMember(Name = "coverage_end_date")]
		public string CoverageEndDate { get; set; }
	}
}