using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

/// <summary>
/// Represents the base response.
/// </summary>
[DataContract]
public class BaseResponse
{
	/// <summary>
	/// Total count of records.
	/// </summary>
	[DataMember(Name = "totalCount")]
	public int TotalCount { get; set; }
}
