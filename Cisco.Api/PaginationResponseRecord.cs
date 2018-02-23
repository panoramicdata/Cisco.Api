namespace Cisco.Api
{
	public class PaginationResponseRecord
	{
		public int PageIndex { get; set; }
		public int LastIndex { get; set; }
		public int TotalRecords { get; set; }
		public int PageRecords { get; set; }
	}
}