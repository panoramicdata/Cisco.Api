using System;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace Cisco.Api.Security;

internal static partial class ReadOnlyGuard
{
	// Cisco reads some Smart Accounts and Licensing usage data with POST, so those operations are allowed by name.
	[GeneratedRegex("/v1/accounts/[^/]+/(license-subscriptions|licenses)$", RegexOptions.IgnoreCase)]
	private static partial Regex ReadOnlyPostPath();

	internal static bool IsReadRequest(HttpRequestMessage request)
	{
		if (request.Method == HttpMethod.Get || request.Method == HttpMethod.Head || request.Method == HttpMethod.Options)
		{
			return true;
		}

		if (request.Method != HttpMethod.Post)
		{
			return false;
		}

		var uri = request.RequestUri;
		var path = uri is null ? string.Empty : uri.IsAbsoluteUri ? uri.AbsolutePath : uri.OriginalString.Split('?')[0];
		return ReadOnlyPostPath().IsMatch(path.TrimEnd('/'));
	}

	internal static void ThrowIfRefused(CiscoClientOptions options, HttpRequestMessage request)
	{
		if (options.EffectiveIsReadOnly && !IsReadRequest(request))
		{
			throw new InvalidOperationException($"{request.Method} {request.RequestUri} is refused because CiscoClientOptions.IsReadOnly is set.");
		}
	}
}
