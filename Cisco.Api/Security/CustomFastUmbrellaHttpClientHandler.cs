using Cisco.Api.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

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
internal abstract class CustomFastUmbrellaHttpClientHandler(
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

	private async Task<string> GetAccessTokenAsync(int currentCredentialCurrentIndex, CancellationToken cancellationToken)
	{
		_logger.LogDebug("Authenticating...");

		var attemptCount = 0;

		while (true)
		{
			using var httpClient = GetHttpClient(currentCredentialCurrentIndex);

			HttpResponseMessage httpResponseMessage;
			try
			{
				httpResponseMessage = await httpClient
					.PostAsync(string.Empty, GetAuthBody(), cancellationToken)
					.ConfigureAwait(false);

				_logger.LogTrace("{HttpResponseMessage}", httpResponseMessage);
				}
				catch (Exception ex) when (IsTransientException(ex))
				{
					if (++attemptCount < Options.MaxAttemptCount)
					{
						_logger.LogWarning("GetAccessTokenAsync(): Attempt {AttemptCount}/{MaxAttemptCount} failed, retrying...",
							attemptCount,
							Options.MaxAttemptCount
						);
						await Task.Delay(Options.RetryDelay, cancellationToken).ConfigureAwait(false);
						continue;
					}

					_logger.LogError(ex, "GetAccessTokenAsync(): {Message} after {MaxAttemptCount} attempts.",
						ex.Message, Options.MaxAttemptCount);
					throw new CiscoApiException("Timeout or transient network failure during authentication.", ex);
				}

				var accessTokenResponse = await DeserializeTokenResponseAsync(httpResponseMessage, cancellationToken).ConfigureAwait(false);

			if (accessTokenResponse.Error is not null)
			{
				var attemptCountRef = new[] { attemptCount };
				var shouldContinue = await HandleAuthErrorAsync(currentCredentialCurrentIndex, accessTokenResponse, httpResponseMessage, attemptCountRef, cancellationToken)
					.ConfigureAwait(false);
				attemptCount = attemptCountRef[0];
				if (shouldContinue)
				{
					continue;
				}
			}

			return StoreAndReturnAccessToken(currentCredentialCurrentIndex, accessTokenResponse);
		}
	}

	private static bool IsTransientException(Exception ex)
		=> ex is TaskCanceledException
			or HttpRequestException
			or TimeoutException
			or SocketException
			|| (ex is IOException ioEx && ioEx.InnerException is SocketException);

	private static async Task<AccessTokenResponse> DeserializeTokenResponseAsync(HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken)
	{
		var contents = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
		return JsonConvert.DeserializeObject<AccessTokenResponse>(contents)
			?? throw new FormatException("Unable to deserialize access token response");
	}

	private async Task<bool> HandleAuthErrorAsync(
		int currentCredentialCurrentIndex,
		AccessTokenResponse accessTokenResponse,
		HttpResponseMessage httpResponseMessage,
		int[] attemptCount,
		CancellationToken cancellationToken)
	{
		var error = accessTokenResponse.Error!;
		var description = accessTokenResponse.ErrorDescription;
		var combinedMessage = BuildErrorMessage(error, description);

		_logger.LogDebug("Authentication failed. Error={Error} Description={Description}", error, description);

		if (!_logger.IsEnabled(LevelToLogAt) && Options.OnErrorEnsureRequestResponseHeadersLogged)
		{
			await LogResponseHeaders(currentCredentialCurrentIndex, httpResponseMessage, true).ConfigureAwait(false);
		}

		var isInvalidClient = error.Equals("invalid_client", StringComparison.OrdinalIgnoreCase);

		if (isInvalidClient)
		{
			if (Options.RetryInvalidClientTokenErrors)
			{
				if (++attemptCount[0] < Options.RetryInvalidClientTokenErrorsMaxAttemptCount)
				{
					_logger.LogWarning("GetAccessTokenAsync(): invalid_client ({AttemptCount}/{MaxAttemptCount}) – retrying after {Delay}s...",
						attemptCount[0],
						Options.RetryInvalidClientTokenErrorsMaxAttemptCount,
						Options.RetryInvalidClientTokenErrorsRetryDelay.TotalSeconds);

					await Task.Delay(Options.RetryInvalidClientTokenErrorsRetryDelay, cancellationToken).ConfigureAwait(false);
					return true;
				}

				_logger.LogError("GetAccessTokenAsync(): invalid_client exhausted after {MaxAttemptCount} attempts.",
					Options.RetryInvalidClientTokenErrorsMaxAttemptCount);
				throw new CiscoApiException("Timeout during authentication - gave up trying to get token after repeated invalid_client errors.");
			}

			if (++attemptCount[0] < Options.MaxAttemptCount)
			{
				_logger.LogWarning("GetAccessTokenAsync(): invalid_client ({AttemptCount}/{MaxAttemptCount}) using standard retry settings, retrying after {Delay}s...",
					attemptCount[0],
					Options.MaxAttemptCount,
					Options.RetryDelay.TotalSeconds);
				await Task.Delay(Options.RetryDelay, cancellationToken).ConfigureAwait(false);
				return true;
			}
		}

		throw new SecurityException(combinedMessage);
	}

	private static string BuildErrorMessage(string error, string? description)
		=> description is { Length: > 0 } ? $"{error}: {description}" : error;

	private string StoreAndReturnAccessToken(int currentCredentialCurrentIndex, AccessTokenResponse accessTokenResponse)
	{
		_logger.LogDebug("Authentication succeeded.");

		var expireInSeconds = accessTokenResponse.ExpiresInSeconds ?? 3540;

		if (accessTokenResponse.ExpiresInSeconds - 60 > 0)
		{
			expireInSeconds -= 60;
		}

		_logger.LogDebug("Access token should expire in {ExpireInSeconds} seconds.", expireInSeconds);

		CiscoUmbrellaCredentials.ElementAt(currentCredentialCurrentIndex).Value.AccessTokenExpiryDateTimeOffset = DateTimeOffset.UtcNow.AddSeconds(expireInSeconds);

		_logger.LogDebug(
			"The access token '{AccessToken}' expiry date time is '{ExpiryDateTimeUtc}'",
			accessTokenResponse.AccessToken!,
			CiscoUmbrellaCredentials.ElementAt(currentCredentialCurrentIndex).Value.AccessTokenExpiryDateTimeOffset
		);

		return accessTokenResponse.AccessToken!;
	}

	protected override async Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		var currentCredentialCurrentIndex = SelectNextCredentialIndex();
		var currentCredentialsInstance = CiscoUmbrellaCredentials.ElementAt(currentCredentialCurrentIndex).Value;

		await SetupRequestAsync(currentCredentialCurrentIndex, currentCredentialsInstance, request, cancellationToken).ConfigureAwait(false);

		// Pre-read the request body into a byte array so it can be replayed on every retry attempt.
		// The content stream on an HttpRequestMessage is forward-only: once HttpClient has consumed it
		// on the first send, it is exhausted and cannot be read again. Without this step, any retry
		// after a transient failure (e.g. an HttpClient timeout or a 5xx response) would send an
		// empty body to the server.
		byte[]? requestBodyBytes = null;
		if (request.Content is not null)
		{
			requestBodyBytes = await request.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
		}

		var attemptCount = 0;
		while (true)
		{
			// A fresh HttpRequestMessage clone is required for every attempt.
			// Once base.SendAsync() has been called — even if it times out or throws — the original
			// HttpRequestMessage is marked as 'already sent' by the framework and will throw
			// InvalidOperationException: "The request message was already sent" if reused.
			// Cloning ensures each attempt gets a clean, unsent message with the same headers and body.
			using var attemptRequest = CloneRequest(request, requestBodyBytes);

			HttpResponseMessage httpResponseMessage;
			try
			{
				httpResponseMessage = await base
					.SendAsync(attemptRequest, cancellationToken)
					.ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				if (++attemptCount < Options.MaxAttemptCount)
				{
					_logger.LogWarning(
						"Attempt {AttemptCount}/{MaxAttemptCount} failed, retrying...",
						attemptCount,
						Options.MaxAttemptCount);

					// Deliberately use CancellationToken.None for the inter-retry delay rather than the
					// caller's cancellationToken. When an HttpClient request times out, the framework
					// cancels its own internal CancellationTokenSource — it does NOT cancel the token
					// supplied by the caller. If we passed cancellationToken here, a timeout on attempt N
					// would cancel the delay and prevent attempt N+1 from ever starting, silently
					// discarding the configured MaxAttemptCount retries. Using CancellationToken.None
					// ensures the delay (and the subsequent retry) always runs after a timeout.
					// The caller's token is still respected on the actual SendAsync call above, so a
					// deliberate job cancellation (e.g. Quartz shutdown) will still stop the retry loop.
					await Task.Delay(Options.RetryDelay, CancellationToken.None)
						.ConfigureAwait(false);

					continue;
				}

				if (!_logger.IsEnabled(LevelToLogAt) && Options.OnErrorEnsureRequestResponseHeadersLogged)
				{
					await LogRequestHeaders(currentCredentialCurrentIndex, request, true).ConfigureAwait(false);
				}

				_logger.LogError(
					ex, "{Message} after {MaxAttemptCount} attempts.",
					ex.Message,
					Options.MaxAttemptCount
					);
				throw new CiscoApiException(ex.Message, ex);
			}

			if (_logger.IsEnabled(LevelToLogAt))
			{
				await LogResponseHeaders(currentCredentialCurrentIndex, httpResponseMessage).ConfigureAwait(false);
			}

			var statusCode = httpResponseMessage.StatusCode;
			var content = httpResponseMessage.Content;
#if DEBUG
			var message = await GetResponseContent(statusCode, content).ConfigureAwait(false);
#endif

			if (!httpResponseMessage.IsSuccessStatusCode)
			{
#if !DEBUG
				var message = await GetResponseContent(statusCode, content).ConfigureAwait(false);
#endif

				var attemptCountRef = new[] { attemptCount };
				if (await HandleRetriableStatusCodeAsync(currentCredentialCurrentIndex, currentCredentialsInstance, httpResponseMessage, message, request, attemptCountRef, cancellationToken).ConfigureAwait(false))
				{
					attemptCount = attemptCountRef[0];
					continue;
				}

				await LogErrorHeadersIfNeeded(currentCredentialCurrentIndex, request, httpResponseMessage).ConfigureAwait(false);

				_logger.LogError(
					"{Message} after {MaxAttemptCount} attempts.",
					message,
					Options.MaxAttemptCount
				);
				var errorContent = await httpResponseMessage
					.Content
					.ReadAsStringAsync(cancellationToken)
					.ConfigureAwait(false);
				throw new CiscoApiException(httpResponseMessage, errorContent);
			}

			return httpResponseMessage;
		}
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

	private async Task EnsureAuthenticatedAsync(int currentCredentialCurrentIndex, CiscoUmbrellaCredentialsTokenTracking currentCredentialsInstance, CancellationToken cancellationToken)
	{
		if (currentCredentialsInstance.AccessTokenExpiryDateTimeOffset is not null
			&& currentCredentialsInstance.AccessTokenExpiryDateTimeOffset <= DateTimeOffset.UtcNow)
		{
			_logger.LogDebug("SendAsync(): The access token expiry date time ('{AccessTokenExpiryDateTimeOffset}') has expired - getting a new token...", currentCredentialsInstance.AccessTokenExpiryDateTimeOffset);
			currentCredentialsInstance.AccessToken = await GetAccessTokenAsync(currentCredentialCurrentIndex, cancellationToken).ConfigureAwait(false);
			currentCredentialsInstance.AuthenticationHeaderValue = new AuthenticationHeaderValue("Bearer", currentCredentialsInstance.AccessToken);
		}

		if (currentCredentialsInstance.AuthenticationHeaderValue is null)
		{
			currentCredentialsInstance.AccessToken = await GetAccessTokenAsync(currentCredentialCurrentIndex, cancellationToken).ConfigureAwait(false);
			currentCredentialsInstance.AuthenticationHeaderValue = new AuthenticationHeaderValue("Bearer", currentCredentialsInstance.AccessToken);
		}
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

	private async Task<bool> HandleRetriableStatusCodeAsync(
		int currentCredentialCurrentIndex,
		CiscoUmbrellaCredentialsTokenTracking currentCredentialsInstance,
		HttpResponseMessage httpResponseMessage,
		string message,
		HttpRequestMessage request,
		int[] attemptCount,
		CancellationToken cancellationToken)
	{
		switch (httpResponseMessage.StatusCode)
		{
			case HttpStatusCode.TooManyRequests:
				if (++attemptCount[0] < Options.MaxAttemptCount)
				{
					var headers = httpResponseMessage.Headers;
					var retryAfter = headers.RetryAfter?.Delta;
					var retryDelay = retryAfter is not null
						? retryAfter.Value
						: Options.RetryDelay;

					_logger.LogWarning(
						"Attempt {AttemptCount}/{MaxAttemptCount} failed due to a 429, retrying in {x} seconds...",
						attemptCount[0],
						Options.MaxAttemptCount,
						retryDelay);

					// Use CancellationToken.None so that a server-side timeout (which cancels HttpClient's
					// internal token, not the caller's) does not abort the delay and prevent the retry.
					await Task.Delay(retryDelay, CancellationToken.None).ConfigureAwait(false);
					return true;
				}

				break;
			case HttpStatusCode.BadGateway:
			case HttpStatusCode.GatewayTimeout:
			case HttpStatusCode.InternalServerError:
			case HttpStatusCode.RequestTimeout:
			case HttpStatusCode.ServiceUnavailable:
			case HttpStatusCode.Unauthorized:
				if (++attemptCount[0] < Options.MaxAttemptCount)
				{
					if (message.Contains("Developer Inactive"))
					{
						_logger.LogDebug("SendAsync(): Response content was Developer Inactive - could be a bad API response, requesting a new token.");
						currentCredentialsInstance.AccessToken = await GetAccessTokenAsync(currentCredentialCurrentIndex, cancellationToken).ConfigureAwait(false);
						currentCredentialsInstance.AuthenticationHeaderValue = new AuthenticationHeaderValue("Bearer", currentCredentialsInstance.AccessToken);
						request.Headers.Authorization = currentCredentialsInstance.AuthenticationHeaderValue;
					}

					_logger.LogWarning(
						"Attempt {AttemptCount}/{MaxAttemptCount} failed, retrying...",
						attemptCount[0],
						Options.MaxAttemptCount);

					// Use CancellationToken.None so that a server-side timeout does not abort the delay and prevent the retry.
					await Task.Delay(Options.RetryDelay, CancellationToken.None).ConfigureAwait(false);
					return true;
				}

				break;
			default:
				break;
		}

		return false;
	}

	private async Task LogErrorHeadersIfNeeded(int currentCredentialCurrentIndex, HttpRequestMessage request, HttpResponseMessage httpResponseMessage)
	{
		if (!_logger.IsEnabled(LevelToLogAt) && Options.OnErrorEnsureRequestResponseHeadersLogged)
		{
			await LogRequestHeaders(currentCredentialCurrentIndex, request, true).ConfigureAwait(false);
			await LogResponseHeaders(currentCredentialCurrentIndex, httpResponseMessage, true).ConfigureAwait(false);
		}
	}

	private async Task LogRequestHeaders(int currentCredentialCurrentIndex, HttpRequestMessage request, bool logAsError = false)
	{
		// Use logging override if set
		_logger.Log(
			logAsError ? LogLevel.Error : LevelToLogAt,
			"Request\r\n{Request}",
			request
		);
		if (request.Content != null)
		{
			var content = await request
				.Content
				.ReadAsStringAsync()
				.ConfigureAwait(false);

			_logger.Log(
				logAsError ? LogLevel.Error : LevelToLogAt,
				"** Token Index {TokenIndex}: RequestContent\r\n{RequestContext}",
				currentCredentialCurrentIndex,
				content
			);
		}
	}

	private async Task LogResponseHeaders(int currentCredentialCurrentIndex, HttpResponseMessage httpResponseMessage, bool logAsError = false)
	{
		// Use logging override if set
		_logger.Log(
			logAsError ? LogLevel.Error : LevelToLogAt,
			"Response\r\n{Response}",
			httpResponseMessage
		);

		if (httpResponseMessage.Content != null)
		{
			var content = await httpResponseMessage
				.Content
				.ReadAsStringAsync()
				.ConfigureAwait(false);

			_logger.Log(
				logAsError ? LogLevel.Error : LevelToLogAt,
				"** Token Index {TokenIndex}: ResponseContent\r\n{ResponseContent}",
				currentCredentialCurrentIndex,
				content);
		}
	}

	private static async Task<string> GetResponseContent(HttpStatusCode statusCode, HttpContent? content)
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
	private static HttpRequestMessage CloneRequest(HttpRequestMessage original, byte[]? bodyBytes)
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

	public abstract HttpClient GetHttpClient(int currentCredentialCurrentIndex);

	public abstract StringContent GetAuthBody();
}