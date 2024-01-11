using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.Umbrella;

[DataContract]
[DebuggerDisplay("{Name}")]
public class Policy
{
	[DataMember(Name = "policyId")]
	public required int PolicyId { get; set; }

	[DataMember(Name = "organizationId")]
	public required int OrganizationId { get; set; }

	[DataMember(Name = "name")]
	public required string Name { get; set; }

	[DataMember(Name = "priority")]
	public required int Priority { get; set; }

	[DataMember(Name = "createdAt")]
	public required DateTime CreatedAt { get; set; }

	[DataMember(Name = "isDefault")]
	public required bool IsDefault { get; set; }
}
