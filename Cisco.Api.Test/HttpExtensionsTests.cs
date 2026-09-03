using Cisco.Api.Security;
using System.Net;
using System.Net.Http.Headers;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Tests for header redaction in diagnostic output.
///
/// <para>
/// The handlers set an Authorization header on every request and then pass the whole
/// <see cref="HttpRequestMessage"/> to the logger. Its <c>ToString()</c> renders every header, so
/// without redaction a usable access token is written wherever those messages end up.
/// </para>
///
/// <para>
/// This is not confined to Trace. <c>LogErrorHeadersIfNeeded</c> fires when Trace is
/// <em>disabled</em> and <c>OnErrorEnsureRequestResponseHeadersLogged</c> is set, logging at Error
/// level, which is the normal production configuration.
/// </para>
///
/// <para>
/// These are pure unit tests. They construct messages directly and require no credentials, no
/// configuration and no live API.
/// </para>
/// </summary>
public class HttpExtensionsTests
{
	/// <summary>
	/// Shaped like a real token so that a partial-redaction bug would be visible, but not a real one.
	/// </summary>
	private const string FakeToken = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOP";

	/// <summary>
	/// The headline case: the bearer token this client sets must not survive into the message.
	/// </summary>
	[Fact]
	public void ToRedactedString_RequestWithBearerToken_DoesNotLeakTheCredential()
	{
		using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.cisco.com/thing");
		request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", FakeToken);

		var rendered = request.ToRedactedString();

		rendered.Should().NotContain(FakeToken);
		rendered.Should().Contain($"Authorization: Bearer <redacted, length {FakeToken.Length}>");
	}

	/// <summary>
	/// The Umbrella handlers authenticate with Basic rather than Bearer.
	/// </summary>
	[Fact]
	public void ToRedactedString_BasicScheme_KeepsTheSchemeAndRedactsTheCredential()
	{
		using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.umbrella.com/thing");
		request.Headers.TryAddWithoutValidation("Authorization", "Basic dXNlcjpwYXNzd29yZA==");

		var rendered = request.ToRedactedString();

		rendered.Should().Contain("Authorization: Basic <redacted, length 20>");
		rendered.Should().NotContain("dXNlcjpwYXNzd29yZA==");
	}

	/// <summary>
	/// Proves the defect being fixed: the framework rendering leaks, the replacement does not.
	/// </summary>
	[Fact]
	public void ToRedactedString_UnlikeToString_DoesNotContainTheToken()
	{
		using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.cisco.com/thing");
		request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", FakeToken);

		request.ToString().Should().Contain(FakeToken, "the framework rendering is what leaked");
		request.ToRedactedString().Should().NotContain(FakeToken);
	}

	/// <summary>
	/// The diagnostically useful parts of the message must survive intact.
	/// </summary>
	[Fact]
	public void ToRedactedString_KeepsMethodUriAndOtherHeaders()
	{
		using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.cisco.com/thing");
		request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", FakeToken);
		request.Headers.TryAddWithoutValidation("Accept", "application/json");

		var rendered = request.ToRedactedString();

		rendered.Should().Contain("Method: POST");
		rendered.Should().Contain("https://api.cisco.com/thing");
		rendered.Should().Contain("Accept: application/json");
		rendered.Should().NotContain(FakeToken);
	}

	/// <summary>
	/// Content headers are rendered too, so they must be redacted on the same terms.
	/// </summary>
	[Fact]
	public void ToRedactedString_RedactsContentHeaders()
	{
		using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.cisco.com/thing")
		{
			Content = new StringContent("{}")
		};
		request.Content!.Headers.TryAddWithoutValidation("X-Api-Key", "s3cr3t-content-header");

		var rendered = request.ToRedactedString();

		rendered.Should().NotContain("s3cr3t-content-header");
		rendered.Should().Contain("<redacted");
		rendered.Should().Contain("Content-Type: text/plain; charset=utf-8");
	}

	/// <summary>
	/// A header added without validation keeps whatever casing the caller used.
	/// </summary>
	/// <param name="headerName">The header name casing under test.</param>
	[Theory]
	[InlineData("authorization")]
	[InlineData("AUTHORIZATION")]
	[InlineData("AuThOrIzAtIoN")]
	public void ToRedactedString_AuthorizationHeader_IsRedactedWhateverTheCasing(string headerName)
	{
		using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.cisco.com/thing");
		request.Headers.TryAddWithoutValidation(headerName, $"Bearer {FakeToken}");

		var rendered = request.ToRedactedString();

		rendered.Should().NotContain(FakeToken);
		rendered.Should().Contain("<redacted");
	}

	/// <summary>
	/// The other standard credential-bearing header names are redacted too.
	/// </summary>
	/// <param name="headerName">The credential-bearing header name under test.</param>
	[Theory]
	[InlineData("Proxy-Authorization")]
	[InlineData("Cookie")]
	[InlineData("X-API-Key")]
	[InlineData("Api-Key")]
	[InlineData("X-Api-Token")]
	[InlineData("X-Auth-Token")]
	public void ToRedactedString_OtherCredentialHeaders_AreRedacted(string headerName)
	{
		const string secret = "s3cr3t-value-that-must-not-be-logged";
		using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.cisco.com/thing");
		request.Headers.TryAddWithoutValidation(headerName, secret);

		var rendered = request.ToRedactedString();

		rendered.Should().NotContain(secret);
		rendered.Should().Contain("<redacted");
	}

	/// <summary>
	/// A vendor may prefix the standard header name rather than using it directly.
	/// </summary>
	/// <param name="headerName">The vendor-prefixed header name under test.</param>
	[Theory]
	[InlineData("X-Samanage-Authorization")]
	[InlineData("X-Vendor-Authorization")]
	public void ToRedactedString_VendorPrefixedAuthorizationHeader_IsRedacted(string headerName)
	{
		using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.cisco.com/thing");
		request.Headers.TryAddWithoutValidation(headerName, $"Bearer {FakeToken}");

		var rendered = request.ToRedactedString();

		rendered.Should().NotContain(FakeToken);
		rendered.Should().Contain($"{headerName}: Bearer <redacted, length {FakeToken.Length}>");
	}

	/// <summary>
	/// A cookie value also contains a space, so treating the text before the first space as a scheme
	/// would preserve the very value being redacted. Only Authorization style headers keep a scheme.
	/// </summary>
	[Fact]
	public void ToRedactedString_CookieValueContainingASpace_IsRedactedWhole()
	{
		const string cookie = "session=abc123def456; HttpOnly";
		using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.cisco.com/thing");
		request.Headers.TryAddWithoutValidation("Cookie", cookie);

		var rendered = request.ToRedactedString();

		rendered.Should().Contain($"Cookie: <redacted, length {cookie.Length}>");
		rendered.Should().NotContain("session=");
	}

	/// <summary>
	/// A credential with no scheme prefix has nothing safe to preserve, so all of it goes.
	/// </summary>
	[Fact]
	public void ToRedactedString_CredentialWithoutAScheme_IsRedactedEntirely()
	{
		using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.cisco.com/thing");
		request.Headers.TryAddWithoutValidation("X-API-Key", "abcdef123456");

		var rendered = request.ToRedactedString();

		rendered.Should().Contain("X-API-Key: <redacted, length 12>");
	}

	/// <summary>
	/// Response rendering goes through the same redaction, so Set-Cookie is covered.
	/// </summary>
	[Fact]
	public void ToRedactedString_ResponseSetCookie_IsRedacted()
	{
		using var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
		response.Headers.TryAddWithoutValidation("Set-Cookie", "session=abc123def456; HttpOnly");

		var rendered = response.ToRedactedString();

		rendered.Should().NotContain("abc123def456");
		rendered.Should().Contain("<redacted");
	}

	/// <summary>
	/// The response status is the most useful part of a failure message and must survive.
	/// </summary>
	[Fact]
	public void ToRedactedString_ResponseKeepsStatusAndReason()
	{
		using var response = new HttpResponseMessage(HttpStatusCode.Forbidden);

		var rendered = response.ToRedactedString();

		rendered.Should().Contain("StatusCode: 403");
		rendered.Should().Contain("Forbidden");
	}

	/// <summary>
	/// A request carrying no credential is rendered with nothing removed.
	/// </summary>
	[Fact]
	public void ToRedactedString_NoCredentialHeaders_RedactsNothing()
	{
		using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.cisco.com/thing");
		request.Headers.TryAddWithoutValidation("Accept", "application/json");

		var rendered = request.ToRedactedString();

		rendered.Should().Contain("Accept: application/json");
		rendered.Should().NotContain("<redacted");
	}
}
