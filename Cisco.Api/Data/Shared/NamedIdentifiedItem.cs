using System.Runtime.Serialization;

namespace Cisco.Api.Data.Shared;

/// <summary>
/// Represents the named identified item.
/// </summary>
[DataContract]
public class NamedIdentifiedItem
{
	/// <summary>
	/// Gets or sets the ID.
	/// </summary>
	[DataMember(Name = "id")]
	public string Id { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the name.
	/// </summary>
	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;
}