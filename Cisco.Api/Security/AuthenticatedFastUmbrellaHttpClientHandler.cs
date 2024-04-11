using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Cisco.Api.Security;

internal class AuthenticatedFastUmbrellaHttpClientHandler(
	Uri authenticationUri,
	CiscoClientOptions options,
	ILogger logger) : CustomFastUmbrellaHttpClientHandler(authenticationUri, options, logger)
{
	public override HttpClient GetHttpClient()
	{
		var httpClient = new HttpClient
		{
			BaseAddress = AuthUri,
			Timeout = TimeSpan.FromSeconds(15)
		};
		var base64EncodedClientIdAndSecret = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{CiscoUmbrellaCredentials.ElementAt(CurrentCredentialCurrentIndex).Key}:{CiscoUmbrellaCredentials.ElementAt(CurrentCredentialCurrentIndex).Value.ClientSecret}"));
		httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {base64EncodedClientIdAndSecret}");
		return httpClient;
	}

	public override StringContent GetAuthBody() => new(
			$"grant_type=client_credentials",
			Encoding.UTF8,
			"application/x-www-form-urlencoded");
}
