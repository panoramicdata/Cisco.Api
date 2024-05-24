using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;

namespace Cisco.Api.Security;

internal class AuthenticatedHttpClientHandler(
	Uri authenticationUri,
	CiscoClientOptions options,
	ILogger logger,
	string? scope = null) : CustomHttpClientHandler(authenticationUri, options, logger)
{
	public override HttpClient GetHttpClient()
	{
		var httpClient = new HttpClient
		{
			BaseAddress = AuthUri,
			Timeout = TimeSpan.FromSeconds(15)
		};
		return httpClient;
	}

	public override StringContent GetAuthBody()
	{
		var optionalScope = string.IsNullOrWhiteSpace(scope) ? string.Empty : $"&scope={scope}";
		return new StringContent(
			$"client_id={Options.ClientId}&grant_type=client_credentials&client_secret={Options.ClientSecret}{optionalScope}",
			Encoding.UTF8,
			"application/x-www-form-urlencoded");
	}
}
