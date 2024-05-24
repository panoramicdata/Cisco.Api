using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public class CustomerSuccessTrack
{
	/// <summary>
	/// Unique identifier of the success track. This value can be null if customerAdmin has not yet approved visibility.
	/// </summary>
	[DataMember(Name = "id")]
	public string? Id { get; set; } = null;

	/// <summary>
	/// Access provided by the customer. Currently API providing only the approved success tracks by the customer and this value will be always true.
	/// </summary>
	[DataMember(Name = "access")]
	public bool Access { get; set; }
}
