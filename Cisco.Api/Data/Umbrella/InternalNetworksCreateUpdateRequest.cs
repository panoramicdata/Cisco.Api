using System.Runtime.Serialization;

namespace Cisco.Api.Data.Umbrella;

/// <summary>
/// Internal Networks Create/Update Request
/// </summary>
[DataContract]
public class InternalNetworksCreateUpdateRequest
{
	/// <summary>
	/// The name of the internal network
	/// </summary>
	[DataMember(Name = "name")]
	public required string Name;

	/// <summary>
	/// The IPv4 address of the internal network
	/// </summary>
	[DataMember(Name = "ipAddress")]
	public required string IpAddress;

	/// <summary>
	/// Specifies the prefix length. The prefix length must be greater than 8 and no more than 32
	/// </summary>
	[DataMember(Name = "prefixLength")]
	public required int PrefixLength;

	/// <summary>
	/// The site ID. For DNS policies, specify the ID of the Site associated with internal network (provide only one of siteId, networkId, or tunnelId).
	/// </summary>
	[DataMember(Name = "siteId")]
	public int? SiteId;

	/// <summary>
	/// The network ID. For Web policies through proxy chaining, specify the ID of the network associated with the internal network (provide only one of siteId, networkId, or tunnelId).
	/// </summary>
	[DataMember(Name = "networkId")]
	public int? NetworkId;

	/// <summary>
	/// The ID of the tunnel. For Web policies through the IPsec tunnel, specify the ID of Tunnel associated with the internal network (provide only one of siteId, networkId, or tunnelId).
	/// </summary>
	[DataMember(Name = "tunnelId")]
	public int? TunnelId;
}