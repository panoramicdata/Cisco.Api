using System.Runtime.Serialization;

namespace Cisco.Api.Data.Hello;

/// <summary>
/// A hello response
/// </summary>
[DataContract]
public class Response
{
	/// <summary>
	/// The hello response's hello response.
	/// </summary>
	[DataMember(Name = "helloResponse")]
	public HelloResponse HelloResponse { get; set; } = null!;
}
