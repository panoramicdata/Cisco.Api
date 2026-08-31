namespace Cisco.Api;

/// <summary>
/// Represents the Cisco client credentials.
/// </summary>
public class CiscoClientCredentials
{
	/// <summary>
	/// The API Client Id
	/// </summary>
	public required string ClientId { get; set; }

	/// <summary>
	/// The API Client secret
	/// </summary>
	public required string ClientSecret { get; set; }
}