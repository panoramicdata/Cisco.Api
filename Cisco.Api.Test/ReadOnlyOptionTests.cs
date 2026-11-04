using Cisco.Api.Security;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Pure unit tests: a refused write throws before any request is sent, so no credentials or network are needed.
/// </summary>
public class ReadOnlyOptionTests
{
	private static CiscoClientOptions NewOptions(bool isReadOnly)
		=> new() { ClientId = "fake-id", ClientSecret = "fake-secret", IsReadOnly = isReadOnly };

	/// <summary>
	/// The option defaults to false so existing callers are unaffected.
	/// </summary>
	[Fact]
	public void IsReadOnly_DefaultsToFalse()
		=> new CiscoClientOptions().IsReadOnly.Should().BeFalse();

	/// <summary>
	/// A write sent through the real handler is refused before authentication or sending.
	/// </summary>
	[Theory]
	[InlineData("POST", "https://api-cx.cisco.com/px/v1/customers/1/reports")]
	[InlineData("PUT", "https://api.umbrella.com/deployments/v2/internalnetworks/1")]
	[InlineData("DELETE", "https://api.umbrella.com/deployments/v2/internalnetworks/1")]
	public async Task WriteIsRefused_WhenIsReadOnly(string method, string url)
	{
		using var handler = new AuthenticatedHttpClientHandler(new("https://id.cisco.invalid/token"), NewOptions(true), Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance);
		using var client = new HttpClient(handler);

		var act = () => client.SendAsync(new HttpRequestMessage(new HttpMethod(method), url), TestContext.Current.CancellationToken);

		await act.Should().ThrowAsync<InvalidOperationException>();
	}

	/// <summary>
	/// The Umbrella handler enforces the option too.
	/// </summary>
	[Fact]
	public async Task UmbrellaDeleteIsRefused_WhenIsReadOnly()
	{
		using var handler = new AuthenticatedUmbrellaHttpClientHandler(new("https://id.cisco.invalid/token"), NewOptions(true), Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance);
		using var client = new HttpClient(handler);

		var act = () => client.SendAsync(new HttpRequestMessage(HttpMethod.Delete, "https://api.umbrella.com/deployments/v2/internalnetworks/1"), TestContext.Current.CancellationToken);

		await act.Should().ThrowAsync<InvalidOperationException>();
	}

	/// <summary>
	/// The copy of the options handed to a per-API client follows the caller's setting live.
	/// </summary>
	[Fact]
	public void DerivedOptions_FollowTheSourceSettingLive()
	{
		var source = NewOptions(false);
		var derived = new CiscoClientOptions { ReadOnlySource = source };

		derived.EffectiveIsReadOnly.Should().BeFalse();

		source.IsReadOnly = true;

		derived.EffectiveIsReadOnly.Should().BeTrue();
	}

	/// <summary>
	/// Reads, including the POST usage queries, are allowed; every other write is not.
	/// </summary>
	[Theory]
	[InlineData("GET", "https://apix.cisco.com/eox/1", true)]
	[InlineData("POST", "https://swapi.cisco.com/services/api/smart-accounts-and-licensing/v1/accounts/a.com/license-subscriptions", true)]
	[InlineData("POST", "https://swapi.cisco.com/services/api/smart-accounts-and-licensing/v1/accounts/a.com/licenses", true)]
	[InlineData("POST", "https://api-cx.cisco.com/px/v1/customers/1/reports", false)]
	[InlineData("POST", "https://api.umbrella.com/deployments/v2/internalnetworks", false)]
	[InlineData("PUT", "https://api.umbrella.com/deployments/v2/policies/1/identities/2", false)]
	[InlineData("DELETE", "https://api.umbrella.com/deployments/v2/internalnetworks/1", false)]
	public void IsReadRequest_ClassifiesRequests(string method, string url, bool expected)
	{
		using var request = new HttpRequestMessage(new HttpMethod(method), url);

		ReadOnlyGuard.IsReadRequest(request).Should().Be(expected);
	}
}
