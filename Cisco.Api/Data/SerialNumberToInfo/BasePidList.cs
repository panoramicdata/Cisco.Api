using System.Runtime.Serialization;

namespace Cisco.Api.Data.SerialNumberToInfo
{
	/// <summary>
	/// A base pid list
	/// </summary>
	[DataContract]
	public class BasePidList
	{
		/// <summary>
		/// The base pid
		/// </summary>
		[DataMember(Name = "base_pid")]
		public string BasePid { get; set; } = null!;
	}
}