using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.Umbrella;

/// <summary>
/// Represents the policy.
/// </summary>
[DataContract]
[DebuggerDisplay("{Name}")]
public class Policy
{
	/// <summary>
	/// Gets or sets the policy ID.
	/// </summary>
	[DataMember(Name = "policyId")]
	public required int PolicyId { get; set; }

	/// <summary>
	/// Gets or sets the organization ID.
	/// </summary>
	[DataMember(Name = "organizationId")]
	public required int OrganizationId { get; set; }

	/// <summary>
	/// Gets or sets the name.
	/// </summary>
	[DataMember(Name = "name")]
	public required string Name { get; set; }

	/// <summary>
	/// Gets or sets the priority.
	/// </summary>
	[DataMember(Name = "priority")]
	public required int Priority { get; set; }

	/// <summary>
	/// Gets or sets the created at.
	/// </summary>
	[DataMember(Name = "createdAt")]
	public required DateTime CreatedAt { get; set; }

	/// <summary>
	/// Gets or sets the is default flag.
	/// </summary>
	[DataMember(Name = "isDefault")]
	public required bool IsDefault { get; set; }
}
