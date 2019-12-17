using System.Runtime.Serialization;

namespace Cisco.Api
{
	[DataContract]
	public class SerialNumberOrderablePid
	{
		[DataMember(Name = "orderable_pid")]
		public string OrderablePid { get; set; }

		[DataMember(Name = "pillar_code")]
		public string PillarCode { get; set; }

		[DataMember(Name = "pillar_description")]
		public string PillarDescription { get; set; }
	}
}