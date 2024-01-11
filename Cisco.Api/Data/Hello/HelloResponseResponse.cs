namespace Cisco.Api.Data.Hello;

/// <summary>
/// A hello response
/// </summary>
public class HelloResponse
{
	/// <summary>
	/// The response.  Should always be "Hello World!".
	/// </summary>
	public string Response { get; set; } = null!;
}