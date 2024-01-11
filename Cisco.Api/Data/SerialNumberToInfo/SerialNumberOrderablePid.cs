using System.Runtime.Serialization;

namespace Cisco.Api.Data.SerialNumberToInfo;

/// <summary>
/// A serial number orderable PID
/// </summary>
[DataContract]
public class SerialNumberOrderablePid
{
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

	/// <summary>
	/// The pillar descripion
	/// </summary>
	[DataMember(Name = "pillar_description")]
	public string PillarDescription { get; set; } = null!;
}