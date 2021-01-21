using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SerialNumberToInfo
{
	/// <summary>
	/// A coverage status collection
	/// </summary>
	[DataContract]
	public class CoverageStatusCollection
	{
		/// <summary>
		/// The coverage statuses
		/// </summary>
		[DataMember(Name = "serial_numbers")]
		public List<CoverageStatus> CoverageStatuses { get; set; } = null!;
	}
}