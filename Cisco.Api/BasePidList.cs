using System.Runtime.Serialization;

namespace Cisco.Api
{
	[DataContract]
	public class BasePidList
	{
		[DataMember(Name = "base_pid")]
		public string BasePid { get; set; }
	}
}