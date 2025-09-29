using System;
using System.Collections.Generic;

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
	/// The maximum number of query attempts - defaults to 1
	/// </summary>
	public int MaxAttemptCount { get; set; } = 1;

	/// <summary>
	/// Delay between retry attempts - defaults to 5 seconds
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

	/// <summary>
	/// This property is only for un-official use with Umbrella client to improve performance.
	/// </summary>
	public List<CiscoClientCredentials>? ClientCredentialsNotSupported { get; set; }

	/// <summary>
	/// An optional User-Agent string to attach to outgoing requests.
	/// </summary>
	public string? UserAgent { get; set; }

	/// <summary>
	/// If you know that any "invalid_client" token errors are temporary false-positives, then attempt to retry them using MaxAttemptCount.
	/// </summary>
	public bool RetryInvalidClientTokenErrors { get; set; }

	/// <summary>
	/// The maximum number of attempts to gain a token. Only used if RetryInvalidClientTokenErrors is true - defaults to 360
	/// Defaults give you a one hour window.
	/// </summary>
	public int RetryInvalidClientTokenErrorsMaxAttemptCount { get; set; } = 1;

	/// <summary>
	/// Delay between retry attempts to gain a token. Only used if RetryInvalidClientTokenErrors is true - defaults to 10 seconds
	/// Defaults give you a one hour window.
	/// </summary>
	public TimeSpan RetryInvalidClientTokenErrorsRetryDelay { get; set; } = TimeSpan.FromSeconds(5);
}