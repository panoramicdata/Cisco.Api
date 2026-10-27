using Cisco.Api.Security;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net;
using System.Text;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// CAEP-2580 The access token is requested with the configured User-Agent, as API requests are, and with none when
/// none is configured. Offline: the token endpoint is a stub.
/// </summary>
public class TokenRequestUserAgentTests
{
	private const string ResearchUserAgent = "NetworkIntelligenceResearch/1.0 boxxe Limited";

	private static readonly Uri TokenUri = new("https://id.cisco.com/oauth2/default/v1/token");

	private static CiscoClientOptions Options(string? userAgent) => new()
	{
		ClientId = "client-id",
		ClientSecret = "client-secret",
		ClientCredentialsNotSupported = [new CiscoClientCredentials { ClientId = "client-id", ClientSecret = "client-secret" }],
		UserAgent = userAgent,
	};

	/// <summary>
	/// Verifies that the token request carries the configured user agent.
	/// </summary>
	[Fact]
	public async Task TokenRequest_CarriesTheConfiguredUserAgent()
	{
		using var tokenEndpoint = new StubTokenEndpoint();
		using var handler = new StubbedTokenHandler(Options(ResearchUserAgent), tokenEndpoint);

		await handler.GetAccessTokenAsync(TestContext.Current.CancellationToken);

		tokenEndpoint.UserAgents.Should().Equal(ResearchUserAgent);
	}

	/// <summary>
	/// Verifies that the token request carries no user agent when none is configured.
	/// </summary>
	[Fact]
	public async Task TokenRequest_CarriesNoUserAgent_WhenNoneIsConfigured()
	{
		using var tokenEndpoint = new StubTokenEndpoint();
		using var handler = new StubbedTokenHandler(Options(null), tokenEndpoint);

		await handler.GetAccessTokenAsync(TestContext.Current.CancellationToken);

		tokenEndpoint.UserAgents.Should().Equal(string.Empty);
	}

	/// <summary>
	/// Verifies that the multi-key Umbrella token request carries the configured user agent.
	/// </summary>
	[Fact]
	public async Task UmbrellaTokenRequest_CarriesTheConfiguredUserAgent()
	{
		using var tokenEndpoint = new StubTokenEndpoint();
		using var handler = new StubbedUmbrellaTokenHandler(Options(ResearchUserAgent), tokenEndpoint);

		await handler.GetAccessTokenAsync(0, TestContext.Current.CancellationToken);

		tokenEndpoint.UserAgents.Should().Equal(ResearchUserAgent);
	}

	private sealed class StubbedTokenHandler(CiscoClientOptions options, StubTokenEndpoint tokenEndpoint)
		: AuthenticatedHttpClientHandler(TokenUri, options, NullLogger.Instance)
	{
		public override HttpClient GetHttpClient() => new(tokenEndpoint, disposeHandler: false) { BaseAddress = TokenUri };
	}

	private sealed class StubbedUmbrellaTokenHandler(CiscoClientOptions options, StubTokenEndpoint tokenEndpoint)
		: AuthenticatedFastUmbrellaHttpClientHandler(TokenUri, options, NullLogger.Instance)
	{
		public override HttpClient GetHttpClient(int currentCredentialCurrentIndex) => new(tokenEndpoint, disposeHandler: false) { BaseAddress = TokenUri };
	}

	private sealed class StubTokenEndpoint : HttpMessageHandler
	{
		public List<string> UserAgents { get; } = [];

		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			UserAgents.Add(request.Headers.UserAgent.ToString());
			return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new StringContent("""{"access_token":"token-1","token_type":"Bearer","expires_in":3600}""", Encoding.UTF8, "application/json"),
			});
		}
	}
}
