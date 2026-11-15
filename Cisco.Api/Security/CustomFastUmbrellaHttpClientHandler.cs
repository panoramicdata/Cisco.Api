using Cisco.Api.Exceptions;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Net.Http.Headers;


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

		var credentialIndex = SelectNextCredentialIndex();
		var credentials = CiscoUmbrellaCredentials.ElementAt(credentialIndex).Value;

		await SetupRequestAsync(credentialIndex, credentials, request, cancellationToken).ConfigureAwait(false);

		return await SendLoop.RunAsync(
			request,
			new SendSteps(
				(attemptRequest, original, attempts) => SendAttemptAsync(credentialIndex, attemptRequest, original, attempts, cancellationToken),
				response => LogResponseHeadersIfNeeded(credentialIndex, response),
				(response, message, original, attempts) => HandleRetriableStatusCodeAsync(credentialIndex, credentials, response, message, original, attempts, cancellationToken),
				(original, response, message, attempts) => CreateResponseExceptionAsync(credentialIndex, original, response, message, attempts, cancellationToken)),
			cancellationToken).ConfigureAwait(false);
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
		return await HandlerSupport.BuildResponseExceptionAsync(_logger, response, message, attemptCount, cancellationToken).ConfigureAwait(false);
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

		HandlerSupport.AddUserAgent(request, Options);
	}

	public abstract HttpClient GetHttpClient(int currentCredentialCurrentIndex);

	public abstract StringContent GetAuthBody();
}
