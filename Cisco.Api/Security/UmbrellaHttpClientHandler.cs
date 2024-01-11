using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;

namespace Cisco.Api.Security;

internal class UmbrellaHttpClientHandler(
	Uri authenticationUri,
	CiscoClientOptions options,
	ILogger logger) : CustomHttpClientHandler(authenticationUri, options, logger)
{
	public override HttpClient GetHttpClient()
	{
		var httpClient = new HttpClient
		{
			BaseAddress = AuthUri,
			Timeout = TimeSpan.FromSeconds(15)
		};
		var base64EncodedClientIdAndSecret = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Options.ClientId}:{Options.ClientSecret}"));
		httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {base64EncodedClientIdAndSecret}");
		return httpClient;
	}

	public override StringContent GetAuthBody() => new(
			$"grant_type=client_credentials",
			Encoding.UTF8,
			"application/x-www-form-urlencoded");
}
