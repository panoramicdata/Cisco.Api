using System.Collections.Generic;

namespace Cisco.Api
{
	public class EoxInfoPage
	{
		public PaginationResponseRecord PaginationResponseRecord { get; set; }
		public List<EoxRecord> EoxRecord { get; set; }
	}
}