using System;
using System.Net.Http.Headers;

namespace Cisco.Api;

internal class CiscoUmbrellaCredentialsTokenTracking
{
	//private string clientSecret;

	public CiscoUmbrellaCredentialsTokenTracking(string clientSecret)
	{
		ClientSecret = clientSecret;
	}

	public string ClientSecret { get; set; }

	public string? AccessToken { get; set; }

	public DateTimeOffset? AccessTokenExpiryDateTimeOffset { get; set; }

	public AuthenticationHeaderValue? AuthenticationHeaderValue { get; set; }
}