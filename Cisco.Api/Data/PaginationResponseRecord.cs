using System.Runtime.Serialization;

namespace Cisco.Api.Data
{
	[DataContract]
	public class PaginationResponseRecord
	{
		/// <summary>
		/// Page index
		/// </summary>
		[DataMember(Name = "pageIndex")]
		public int PageIndex { get; set; }

		/// <summary>
		/// Last index
		/// </summary>
		[DataMember(Name = "lastIndex")]
		public int LastIndex { get; set; }

		/// <summary>
		/// Total records
		/// </summary>
		[DataMember(Name = "totalRecords")]
		public int TotalRecords { get; set; }

		/// <summary>
		/// Page records
		/// </summary>
		[DataMember(Name = "pageRecords")]
		public int PageRecords { get; set; }
	}
}