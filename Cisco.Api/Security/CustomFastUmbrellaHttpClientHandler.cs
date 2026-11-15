using Cisco.Api.Exceptions;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Net.Http.Headers;

using static Cisco.Api.Security.HandlerSupport;

namespace Cisco.Api.Security;

/// <summary>
/// This is an alternative to CustomUmbrellaHttpClientHandler that allows you to utilise multiple API keys for Umbrella, to reduce the impact
/// of rate limiting that prevents more than 720 requests per hour per API key.
/// Each query cycles through the API keys, so providing 10 sets of API credentials is probably a good start.
/// Instead of using Options ClientId and ClientSecret, instead set Credentials property. This only works for the Umbrella client.
/// Umbrella may solve this in the future, but for now, this is a workaround.
/// </summary>
/// <param name="authenticationUri"></param>
/// <param name="options"></param>
/// <param name="logger"></param>
internal abstract partial class CustomFastUmbrellaHttpClientHandler(
	Uri authenticationUri,
	CiscoClientOptions options,
	ILogger logger) : HttpClientHandler
{
	private readonly ILogger _logger = logger;

	private const LogLevel LevelToLogAt = LogLevel.Trace;

	protected Uri AuthUri { get; } = authenticationUri;

	protected CiscoClientOptions Options { get; } = options;

	// Assign the credentials to a ConcurrentDictionary to allow for thread-safe access
	protected ConcurrentDictionary<string, CiscoUmbrellaCredentialsTokenTracking> CiscoUmbrellaCredentials { get; set; }
	  = new ConcurrentDictionary<string, CiscoUmbrellaCredentialsTokenTracking>(
			 options.ClientCredentialsNotSupported!.ToDictionary(x => x.ClientId, x => new CiscoUmbrellaCredentialsTokenTracking(x.ClientSecret)));

	protected int CurrentCredentialCurrentIndex { get; set; }

	public bool IsFirstQuery { get; private set; } = true;

	protected override async Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		ReadOnlyGuard.ThrowIfRefused(Options, request);

		var currentCredentialCurrentIndex = SelectNextCredentialIndex();
		var currentCredentialsInstance = CiscoUmbrellaCredentials.ElementAt(currentCredentialCurrentIndex).Value;

		await SetupRequestAsync(currentCredentialCurrentIndex, currentCredentialsInstance, request, cancellationToken).ConfigureAwait(false);
		var requestBodyBytes = await ReadRequestBodyAsync(request, cancellationToken).ConfigureAwait(false);
		var attemptCount = 0;
		while (true)
		{
			using var attemptRequest = CloneRequest(request, requestBodyBytes);
			var attempt = await SendAttemptAsync(currentCredentialCurrentIndex, attemptRequest, request, attemptCount, cancellationToken).ConfigureAwait(false);
			attemptCount = attempt.AttemptCount;
			if (attempt.Response is null)
			{
				continue;
			}

			await LogResponseHeadersIfNeeded(currentCredentialCurrentIndex, attempt.Response).ConfigureAwait(false);
			if (attempt.Response.IsSuccessStatusCode)
			{
				return attempt.Response;
			}

			var message = await GetResponseContent(attempt.Response.StatusCode, attempt.Response.Content).ConfigureAwait(false);
			var attemptCountRef = new[] { attemptCount };
			if (await HandleRetriableStatusCodeAsync(currentCredentialCurrentIndex, currentCredentialsInstance, attempt.Response, message, request, attemptCountRef, cancellationToken).ConfigureAwait(false))
			{
				attemptCount = attemptCountRef[0];
				continue;
			}

			// attemptCount counts the failures before this response, so this response was attempt attemptCount + 1.
			throw await CreateResponseExceptionAsync(currentCredentialCurrentIndex, request, attempt.Response, message, attemptCount + 1, cancellationToken).ConfigureAwait(false);
		}
	}

	private async Task<CiscoApiException> CreateResponseExceptionAsync(
		int credentialIndex,
		HttpRequestMessage request,
		HttpResponseMessage response,
		string message,
		int attemptCount,
		CancellationToken cancellationToken)
	{
		await LogErrorHeadersIfNeeded(credentialIndex, request, response).ConfigureAwait(false);
		_logger.LogFinalFailure(null, message, attemptCount);
		var errorContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
		return new CiscoApiException(response, errorContent);
	}

	private int SelectNextCredentialIndex()
	{
		if (IsFirstQuery)
		{
			IsFirstQuery = false;
		}
		else
		{
			CurrentCredentialCurrentIndex = (CurrentCredentialCurrentIndex + 1) % CiscoUmbrellaCredentials.Count;
		}

		return CurrentCredentialCurrentIndex;
	}

	private async Task SetupRequestAsync(int currentCredentialCurrentIndex, CiscoUmbrellaCredentialsTokenTracking currentCredentialsInstance, HttpRequestMessage request, CancellationToken cancellationToken)
	{
		await EnsureAuthenticatedAsync(currentCredentialCurrentIndex, currentCredentialsInstance, cancellationToken).ConfigureAwait(false);

		request.Headers.Authorization = currentCredentialsInstance.AuthenticationHeaderValue;
		request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

		if (_logger.IsEnabled(LevelToLogAt))
		{
			await LogRequestHeaders(currentCredentialCurrentIndex, request).ConfigureAwait(false);
		}

		if (Options.UserAgent is not null)
		{
			request.Headers.Add("User-Agent", Options.UserAgent);
		}
	}

	public abstract HttpClient GetHttpClient(int currentCredentialCurrentIndex);

	public abstract StringContent GetAuthBody();
}
