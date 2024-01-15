using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.Umbrella;

[DataContract]
[DebuggerDisplay("{Name} ({IpAddress}/{PrefixLength})")]
public class InternalNetwork
{
	/// <summary>
	/// The origin ID of the internal network.
	/// </summary>
	[DataMember(Name = "originId")]
	public required int OriginId { get; set; }

	/// <summary>
	/// The name of the internal network.
	/// </summary>
	[DataMember(Name = "name")]
	public required string Name { get; set; }

	/// <summary>
	/// The IPv4 address of the internal network.
	/// </summary>
	[DataMember(Name = "ipAddress")]
	public required string IpAddress { get; set; }

	/// <summary>
	/// Specifies the prefix length of the internal network. The prefix length must be greater than 8 and no more than 32.
	/// </summary>
	[DataMember(Name = "prefixLength")]
	public required int PrefixLength { get; set; }

	/// <summary>
	/// The name of the site associated with an internal network.
	/// </summary>
	[DataMember(Name = "siteName")]
	public string? SiteName { get; set; }

	/// <summary>
	/// The ID of the site associated with an internal network.
	/// </summary>
	[DataMember(Name = "siteId")]
	public int? SiteId { get; set; }

	/// <summary>
	/// The name of the network associated with an internal network.
	/// </summary>
	[DataMember(Name = "networkName")]
	public string? NetworkName { get; set; }

	/// <summary>
	/// The ID of the network associated with an internal network.
	/// </summary>
	[DataMember(Name = "networkId")]
	public int? NetworkId { get; set; }

	/// <summary>
	/// The name of the tunnel associated with an internal network.
	/// </summary>
	[DataMember(Name = "tunnelName")]
	public string? TunnelName { get; set; }

	/// <summary>
	/// The ID of the tunnel associated with an internal network.
	/// </summary>
	[DataMember(Name = "tunnelId")]
	public int? TunnelId { get; set; }

	/// <summary>
	/// The date and time (ISO8601 timestamp) when the internal network was created.
	/// </summary>
	[DataMember(Name = "createdAt")]
	public DateTime? CreatedAt { get; set; }

	/// <summary>
	/// The date and time (ISO8601 timestamp) when the internal network was modified.
	/// </summary>
	[DataMember(Name = "modifiedAt")]
	public DateTime? ModifiedAt { get; set; }
}