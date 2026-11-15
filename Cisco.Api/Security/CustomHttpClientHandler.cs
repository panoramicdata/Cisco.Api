using Cisco.Api.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Security;

internal abstract partial class CustomHttpClientHandler(
	Uri authenticationUri,
	CiscoClientOptions options,
	ILogger logger) : HttpClientHandler
{
	private readonly ILogger _logger = logger;

	private const LogLevel LevelToLogAt = LogLevel.Trace;

	private readonly bool _useJsonContentType = options.UseJsonContentType;

	protected Uri AuthUri { get; } = authenticationUri;

	protected CiscoClientOptions Options { get; } = options;

	protected override async Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		ReadOnlyGuard.ThrowIfRefused(Options, request);

		await SetupRequestAsync(request, cancellationToken).ConfigureAwait(false);

		return await SendLoop.RunAsync(
			request,
			new SendSteps(
				(attemptRequest, original, attempts) => SendAttemptAsync(attemptRequest, original, attempts, cancellationToken),
				LogResponseHeadersIfNeeded,
				(response, message, original, attempts) => HandleRetriableStatusCodeAsync(response, message, original, attempts, cancellationToken),
				(original, response, message, attempts) => CreateResponseExceptionAsync(original, response, message, attempts, cancellationToken)),
			cancellationToken).ConfigureAwait(false);
	}

	private async Task<CiscoApiException> CreateResponseExceptionAsync(
		HttpRequestMessage request,
		HttpResponseMessage response,
		string message,
		int attemptCount,
		CancellationToken cancellationToken)
	{
		await LogErrorHeadersIfNeeded(request, response).ConfigureAwait(false);
		return await BuildResponseExceptionAsync(_logger, response, message, attemptCount, cancellationToken).ConfigureAwait(false);
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

		HandlerSupport.AddUserAgent(request, Options);
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
