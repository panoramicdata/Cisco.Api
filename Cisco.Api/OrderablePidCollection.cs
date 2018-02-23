using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api
{
	[DataContract]
	public class OrderablePidCollection
	{
		[DataMember(Name="serial_numbers")]
		public List<SerialNumberOrderablePidList> SerialNumberOrderablePids { get; set; }
	}
}