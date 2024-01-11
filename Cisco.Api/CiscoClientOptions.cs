using System;

namespace Cisco.Api;

/// <summary>
/// CiscoClient options
/// </summary>
public class CiscoClientOptions
{
	/// <summary>
	/// The token to use
	/// </summary>
	public string? Token { get; set; }

	/// <summary>
	/// The Client ID.  Must be provided when no HttpClient is provided.
	/// </summary>
	public string? ClientId { get; set; }

	/// <summary>
	/// The Client Secret.  Must be provided when no HttpClient is provided.
	/// </summary>
	public string? ClientSecret { get; set; }

	/// <summary>
	/// The maximum number of query attempts
	/// </summary>
	public int MaxAttemptCount { get; set; } = 1;

	/// <summary>
	/// Delay between retry attempts
	/// </summary>
	public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(5);

	/// <summary>
	/// Allow force logging of headers on error, even if Verbose logging not enabled.
	/// </summary>
	public bool OnErrorEnsureRequestResponseHeadersLogged { get; set; }

	/// <summary>
	/// The HttpClientTimeoutSeconds - defaults to 100
	/// </summary>
	public int HttpClientTimeoutSeconds { get; set; } = 100;
}