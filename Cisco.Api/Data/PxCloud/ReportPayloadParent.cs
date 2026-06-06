using System.Runtime.Serialization;

namespace Cisco.Api.Data.PxCloud;

[DataContract]
public class ReportPayloadParent
{
	/// <summary>
	/// The status.
	/// </summary>
	[DataMember(Name = "metadata")]
	public ReportPayloadMetadata Metadata { get; set; } = null!;
}
