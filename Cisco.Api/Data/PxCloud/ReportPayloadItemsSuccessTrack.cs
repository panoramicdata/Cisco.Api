using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public class ReportPayloadItemsSuccessTrack
{
	/// <summary>
	/// Unique identifier of a Success Track associated with the asset.
	/// </summary>
	[DataMember(Name = "id")]
	public string? Id { get; set; }

	/// <summary>
	/// Unique identifier of the use cases associated with the asset.
	/// </summary>
	[DataMember(Name = "useCases")]
	public List<int> UseCases { get; set; } = [];
}
