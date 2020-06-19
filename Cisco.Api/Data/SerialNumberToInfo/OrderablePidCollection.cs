using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SerialNumberToInfo
{
	/// <summary>
	/// A collection of orderable PIDs
	/// </summary>
	[DataContract]
	public class OrderablePidCollection
	{
		[DataMember(Name = "serial_numbers")]
		public List<SerialNumberOrderablePidList> SerialNumberOrderablePids { get; set; } = null!;
	}
}