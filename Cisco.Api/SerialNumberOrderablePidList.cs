using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api
{
	[DataContract]
	public class SerialNumberOrderablePidList
	{
		[DataMember(Name = "sr_no")]
		public string SerialNumber { get; set; }

		[DataMember(Name = "orderable_pid_list")]
		public List<SerialNumberOrderablePid> OrderablePids { get; set; }
	}
}