using System.Runtime.Serialization;

namespace Cisco.Api
{
	[DataContract]
	public class OrderablePidList
	{
		[DataMember(Name = "item_description")]
		public string ItemDescription { get; set; }

		[DataMember(Name = "item_position")]
		public string ItemPosition { get; set; }

		[DataMember(Name = "item_type")]
		public string ItemType { get; set; }

		[DataMember(Name = "orderable_pid")]
		public string OrderablePid { get; set; }

		[DataMember(Name = "pillar_code")]
		public string PillarCode { get; set; }
	}
}