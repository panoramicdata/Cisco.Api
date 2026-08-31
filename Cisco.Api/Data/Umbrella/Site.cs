using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.Umbrella;

/// <summary>
/// Represents the site.
/// </summary>
[DataContract]
[DebuggerDisplay("{Name}")]
public class Site
{
	/// <summary>
	/// Gets or sets the origin ID.
	/// </summary>
	[DataMember(Name = "originId")]
	public required int OriginId { get; set; }

	/// <summary>
	/// Gets or sets the name.
	/// </summary>
	[DataMember(Name = "name")]
	public required string Name { get; set; }

	/// <summary>
	/// Gets or sets the site ID.
	/// </summary>
	[DataMember(Name = "siteId")]
	public required int SiteId { get; set; }

	/// <summary>
	/// Gets or sets the is default flag.
	/// </summary>
	[DataMember(Name = "isDefault")]
	public required bool IsDefault { get; set; }

	/// <summary>
	/// Gets or sets the modified at.
	/// </summary>
	[DataMember(Name = "modifiedAt")]
	public required DateTime ModifiedAt { get; set; }

	/// <summary>
	/// Gets or sets the created at.
	/// </summary>
	[DataMember(Name = "createdAt")]
	public required DateTime CreatedAt { get; set; }
}
