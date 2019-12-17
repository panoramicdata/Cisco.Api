using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api
{
	[DataContract]
	public class EoxInfoPage
	{
		[DataMember(Name= "PaginationResponseRecord")]
		public PaginationResponseRecord PaginationResponseRecord { get; set; }

		[DataMember(Name = "EOXRecord")]
		public List<EoxRecord> EoxRecords { get; set; }
	}
}