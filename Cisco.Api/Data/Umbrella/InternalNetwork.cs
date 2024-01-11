using System;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.Umbrella;

[DataContract]
public class InternalNetwork
{
	[DataMember(Name = "originId")]
	public required int OriginId { get; set; }

	[DataMember(Name = "ipAddress")]
	public required string IpAddress { get; set; }

	[DataMember(Name = "prefixLength")]
	public required int PrefixLength { get; set; }

	[DataMember(Name = "createdAt")]
	public required DateTime CreatedAt { get; set; }

	[DataMember(Name = "modifiedAt")]
	public required DateTime ModifiedAt { get; set; }

	[DataMember(Name = "name")]
	public required string Name { get; set; }

	[DataMember(Name = "tunnelId")]
	public required int TunnelId { get; set; }

	[DataMember(Name = "tunnelName")]
	public required string TunnelName { get; set; }
}