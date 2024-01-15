using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.Umbrella;

[DataContract]
[DebuggerDisplay("{Name} ({IpAddress}/{PrefixLength})")]
public class InternalNetworkPolicy
{
	/// <summary>
	/// The ID of the policy. Use the policy ID as a reference for subsequent requests
	/// </summary>
	[DataMember(Name = "id")]
	public required int Id { get; set; }

	/// <summary>
	/// The name given to the policy
	/// </summary>
	[DataMember(Name = "name")]
	public required string Name { get; set; }

	/// <summary>
	/// The type of policy. Value values are: dns and web
	/// </summary>
	[DataMember(Name = "type")]
	public string? Type { get; set; }

	/// <summary>
	/// The Umbrella organization ID
	/// </summary>
	[DataMember(Name = "organizationId")]
	public required int OrganizationId { get; set; }

	/// <summary>
	/// A number that represents the priority of the policy in the policy list
	/// </summary>
	[DataMember(Name = "priority")]
	public required int Priority { get; set; }

	/// <summary>
	/// Specifies whether the policy is the default policy
	/// </summary>
	[DataMember(Name = "isDefault")]
	public required bool IsDefault { get; set; }

	/// <summary>
	/// Indicates if policy is directly applied to this identity
	/// </summary>
	[DataMember(Name = "isAppliedDirectly")]
	public required bool IsAppliedDirectly { get; set; }

	/// <summary>
	/// The date and time (ISO8601 timestamp) when the policy was created
	/// </summary>
	[DataMember(Name = "createdAt")]
	public required DateTime CreatedAt { get; set; }

	/// <summary>
	/// The date and time (ISO8601 timestamp) when the policy was modified
	/// </summary>
	[DataMember(Name = "modifiedAt")]
	public required DateTime ModifiedAt { get; set; }

	/// <summary>
	/// The resource URI
	/// </summary>
	[DataMember(Name = "uri")]
	public required string Uri { get; set; }
}