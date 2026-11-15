using Cisco.Api.Exceptions;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Security;

/// <summary>
/// Request and response helpers shared by the Umbrella and standard token handlers.
/// </summary>
internal static class HandlerSupport
{
	internal static bool IsTransientException(Exception ex)
		=> ex is TaskCanceledException
			or HttpRequestException
			or TimeoutException
			or SocketException
			|| (ex is IOException ioEx && ioEx.InnerException is SocketException);

	internal static async Task<AccessTokenResponse> DeserializeTokenResponseAsync(HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken)
	{
		var contents = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
		return JsonConvert.DeserializeObject<AccessTokenResponse>(contents)
			?? throw new FormatException("Unable to deserialize access token response");
	}

	internal static string BuildErrorMessage(string error, string? description)
		=> description is { Length: > 0 } ? $"{error}: {description}" : error;

	internal static Task<byte[]?> ReadRequestBodyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		=> request.Content is null
			? Task.FromResult<byte[]?>(null)
			: ReadContentAsync(request.Content, cancellationToken);

	internal static async Task<byte[]?> ReadContentAsync(HttpContent content, CancellationToken cancellationToken)
		=> await content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);

	internal static bool IsTransientStatusCode(HttpStatusCode statusCode)
		=> statusCode is HttpStatusCode.BadGateway
			or HttpStatusCode.GatewayTimeout
			or HttpStatusCode.InternalServerError
			or HttpStatusCode.RequestTimeout
			or HttpStatusCode.ServiceUnavailable
			or HttpStatusCode.Unauthorized;

	internal static async Task<string> GetResponseContent(HttpStatusCode statusCode, HttpContent? content)
	{
		if (content != null)
		{
			var responseBody = await content
				.ReadAsStringAsync()
				.ConfigureAwait(false);
			return $"{statusCode}: {responseBody}";
		}
		else
		{
			return statusCode.ToString();
		}
	}

	/// <summary>
	/// Creates a fresh, unsent clone of <paramref name="original"/> suitable for a retry attempt.
	/// </summary>
	/// <remarks>
	/// <see cref="HttpRequestMessage"/> is single-use: the .NET HTTP infrastructure marks it as
	/// 'already sent' after the first call to <c>base.SendAsync</c>, even if that call threw or
	/// timed out. Any attempt to pass the same instance to <c>SendAsync</c> a second time throws
	/// <see cref="InvalidOperationException"/>: "The request message was already sent".
	/// <para>
	/// The body bytes are supplied separately because the original <see cref="HttpContent"/> stream
	/// may already be exhausted by the first send attempt; they were pre-read by the caller before
	/// the retry loop began.
	/// </para>
	/// </remarks>
	internal static HttpRequestMessage CloneRequest(HttpRequestMessage original, byte[]? bodyBytes)
	{
		var clone = new HttpRequestMessage(original.Method, original.RequestUri)
		{
			Version = original.Version
		};

		foreach (var header in original.Headers)
		{
			clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
		}

		if (bodyBytes is not null && original.Content is not null)
		{
			var clonedContent = new ByteArrayContent(bodyBytes);
			foreach (var header in original.Content.Headers)
			{
				clonedContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
			}
			clone.Content = clonedContent;
		}

		return clone;
	}
}
