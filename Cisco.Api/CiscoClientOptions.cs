using System;
using System.Net.Http;

namespace Cisco.Api
{
	/// <summary>
	/// CiscoClient options
	/// </summary>
	public class CiscoClientOptions
	{
		/// <summary>
		/// An HttpClient to use instead of the in-built one.
		/// </summary>
		public HttpClient? HttpClient { get; set; }

		/// <summary>
		/// The token to use
		/// </summary>
		public string? Token { get; set; }

		/// <summary>
		/// The URI
		/// </summary>
		public Uri Uri { get; set; } = new Uri("https://api.cisco.com/");

		/// <summary>
		/// The Client ID.  Must be provided when no HttpClient is provided.
		/// </summary>
		public string? ClientId { get; set; }

		/// <summary>
		/// The Client Secret.  Must be provided when no HttpClient is provided.
		/// </summary>
		public string? ClientSecret { get; set; }
	}
}