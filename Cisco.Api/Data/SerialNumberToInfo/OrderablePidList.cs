using System.Runtime.Serialization;

namespace Cisco.Api.Data.SerialNumberToInfo
{
	/// <summary>
	/// An orderable PID list
	/// </summary>
	[DataContract]
	public class OrderablePidList
	{
		/// <summary>
		/// The item description
		/// </summary>
		[DataMember(Name = "item_description")]
		public string ItemDescription { get; set; } = null!;

		/// <summary>
		/// The item position
		/// </summary>
		[DataMember(Name = "item_position")]
		public string ItemPosition { get; set; } = null!;

		/// <summary>
		/// The item type
		/// </summary>
		[DataMember(Name = "item_type")]
		public string ItemType { get; set; } = null!;

		/// <summary>
		/// The orderable PID
		/// </summary>
		[DataMember(Name = "orderable_pid")]
		public string OrderablePid { get; set; } = null!;

		/// <summary>
		/// The pillar code
		/// </summary>
		[DataMember(Name = "pillar_code")]
		public string PillarCode { get; set; } = null!;
	}
}