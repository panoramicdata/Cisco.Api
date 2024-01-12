using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.Umbrella;

[DataContract]
[DebuggerDisplay("{Name}")]
public class Site
{
	[DataMember(Name = "originId")]
	public required int OriginId { get; set; }

	[DataMember(Name = "name")]
	public required string Name { get; set; }

	[DataMember(Name = "siteId")]
	public required int SiteId { get; set; }

	[DataMember(Name = "isDefault")]
	public required bool IsDefault { get; set; }

	[DataMember(Name = "modifiedAt")]
	public required DateTime ModifiedAt { get; set; }

	[DataMember(Name = "createdAt")]
	public required DateTime CreatedAt { get; set; }
}
