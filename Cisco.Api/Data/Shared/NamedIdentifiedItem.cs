using System.Runtime.Serialization;

namespace Cisco.Api.Data.Shared;

[DataContract]
public class NamedIdentifiedItem
{
	[DataMember(Name = "id")]
	public string Id { get; set; } = string.Empty;

	[DataMember(Name = "name")]
	public string Name { get; set; } = string.Empty;
}