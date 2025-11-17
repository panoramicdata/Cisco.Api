using System;
using System.Net.Http.Headers;

namespace Cisco.Api;

internal class CiscoUmbrellaCredentialsTokenTracking(string clientSecret)
{
	//private string clientSecret;


	public string ClientSecret { get; set; } = clientSecret;

	public string? AccessToken { get; set; }

	public DateTimeOffset? AccessTokenExpiryDateTimeOffset { get; set; }

	public AuthenticationHeaderValue? AuthenticationHeaderValue { get; set; }
}