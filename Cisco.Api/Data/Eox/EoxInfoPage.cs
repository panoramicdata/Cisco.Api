using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.Eox
{
	/// <summary>
	/// An EOX info page
	/// </summary>
	[DataContract]
	public class EoxInfoPage
	{
		/// <summary>
		/// The pagination response record
		/// </summary>
		[DataMember(Name = "PaginationResponseRecord")]
		public PaginationResponseRecord PaginationResponseRecord { get; set; } = null!;

		/// <summary>
		/// The EOX records
		/// </summary>
		[DataMember(Name = "EOXRecord")]
		public List<EoxRecord> EoxRecords { get; set; } = null!;
	}
}