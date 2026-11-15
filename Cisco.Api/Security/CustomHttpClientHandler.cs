using Cisco.Api.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Cisco.Api.Security.HandlerSupport;

namespace Cisco.Api.Security;

internal abstract partial class CustomHttpClientHandler(
	Uri authenticationUri,
	CiscoClientOptions options,
	ILogger logger) : HttpClientHandler
{
	private AuthenticationHeaderValue? _authenticationHeaderValue;

	private readonly ILogger _logger = logger;

	private const LogLevel LevelToLogAt = LogLevel.Trace;

	private DateTimeOffset? _accessTokenExpiryDateTimeOffset;

	private readonly bool _useJsonContentType = options.UseJsonContentType;

	protected Uri AuthUri { get; } = authenticationUri;

	protected CiscoClientOptions Options { get; } = options;

	protected override async Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		ReadOnlyGuard.ThrowIfRefused(Options, request);

		await SetupRequestAsync(request, cancellationToken).ConfigureAwait(false);
		var requestBodyBytes = await ReadRequestBodyAsync(request, cancellationToken).ConfigureAwait(false);
		var attemptCount = 0;
		while (true)
		{
			using var attemptRequest = CloneRequest(request, requestBodyBytes);
			var attempt = await SendAttemptAsync(attemptRequest, request, attemptCount, cancellationToken).ConfigureAwait(false);
			attemptCount = attempt.AttemptCount;
			if (attempt.Response is null)
			{
				continue;
			}

			await LogResponseHeadersIfNeeded(attempt.Response).ConfigureAwait(false);
			if (attempt.Response.IsSuccessStatusCode)
			{
				return attempt.Response;
			}

			var message = await GetResponseContent(attempt.Response.StatusCode, attempt.Response.Content).ConfigureAwait(false);
			var attemptCountRef = new[] { attemptCount };
			if (await HandleRetriableStatusCodeAsync(attempt.Response, message, request, attemptCountRef, cancellationToken).ConfigureAwait(false))
			{
				attemptCount = attemptCountRef[0];
				continue;
			}

			// attemptCount counts the failures before this response, so this response was attempt attemptCount + 1.
			throw await CreateResponseExceptionAsync(request, attempt.Response, message, attemptCount + 1, cancellationToken).ConfigureAwait(false);
		}
	}

	private async Task<CiscoApiException> CreateResponseExceptionAsync(
		HttpRequestMessage request,
		HttpResponseMessage response,
		string message,
		int attemptCount,
		CancellationToken cancellationToken)
	{
		await LogErrorHeadersIfNeeded(request, response).ConfigureAwait(false);
		_logger.LogFinalFailure(null, message, attemptCount);
		var errorContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
		return new CiscoApiException(response, errorContent);
	}

	private async Task EnsureAuthenticatedAsync(CancellationToken cancellationToken)
	{
		if (_accessTokenExpiryDateTimeOffset is not null && _accessTokenExpiryDateTimeOffset <= DateTimeOffset.UtcNow)
		{
			_logger.LogDebug("SendAsync(): The access token expiry date time ('{AccessTokenExpiryDateTimeOffset}') has expired - getting a new token...", _accessTokenExpiryDateTimeOffset);
			var accessToken = await GetAccessTokenAsync(cancellationToken).ConfigureAwait(false);
			_authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", accessToken);
		}

		if (_authenticationHeaderValue is null)
		{
			var accessToken = await GetAccessTokenAsync(cancellationToken).ConfigureAwait(false);
			_authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", accessToken);
		}
	}

	private async Task SetupRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		await EnsureAuthenticatedAsync(cancellationToken).ConfigureAwait(false);

		request.Headers.Authorization = _authenticationHeaderValue;
		await PrepareRequestAsync(request, cancellationToken).ConfigureAwait(false);

		if (_logger.IsEnabled(LevelToLogAt))
		{
			await LogRequestHeaders(request).ConfigureAwait(false);
		}

		if (Options.UserAgent is not null)
		{
			request.Headers.Add("User-Agent", Options.UserAgent);
		}
	}

	private async Task PrepareRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		if (_useJsonContentType)
		{
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			string? originalContent = string.Empty;
			if (request.Content != null)
			{
				originalContent = await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
			}

			request.Content = new StringContent(originalContent, Encoding.UTF8, "application/json");
		}
		else
		{
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
		}
	}

	public abstract HttpClient GetHttpClient();

	public abstract StringContent GetAuthBody();
}
