using Cisco.Api.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
	// private AuthenticationHeaderValue? _authenticationHeaderValue;
	private readonly ILogger _logger = logger;
	private const LogLevel LevelToLogAt = LogLevel.Trace;
	// private string? _accessToken = options.Token;
	// private DateTimeOffset? _accessTokenExpiryDateTimeOffset;

	protected Uri AuthUri { get; } = authenticationUri;
	protected CiscoClientOptions Options { get; } = options;

	// Assign the credentials to a ConcurrentDictionary to allow for thread-safe access
	protected ConcurrentDictionary<string, CiscoUmbrellaCredentialsTokenTracking> CiscoUmbrellaCredentials { get; set; }
	  = new ConcurrentDictionary<string, CiscoUmbrellaCredentialsTokenTracking>(
			 options.ClientCredentialsNotSupported!.ToDictionary(x => x.ClientId, x => new CiscoUmbrellaCredentialsTokenTracking(x.ClientSecret)));

	protected int CurrentCredentialCurrentIndex { get; set; } = 0;
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
			// Deal with invalid_client when we intentionally want to push through long periods of server errors until we get a new token
			catch (SecurityException ex) when (
				ex.Message.StartsWith("invalid_client") && Options.RetryInvalidClientTokenErrors)
				{
					if (++attemptCount < Options.RetryInvalidClientTokenErrorsMaxAttemptCount)
					{
						_logger.LogWarning("GetAccessTokenAsync(): Attempt {AttemptCount}/{MaxAttemptCount} failed (invalid_client), retrying...",
							attemptCount,
							Options.RetryInvalidClientTokenErrorsMaxAttemptCount
						);

						await Task.Delay(Options.RetryInvalidClientTokenErrorsRetryDelay, cancellationToken)
							.ConfigureAwait(false);

						continue;
					}

					_logger.LogError(
						ex,
						"GetAccessTokenAsync(): {Message} after {MaxAttemptCount} attempts.",
						ex.Message,
						Options.RetryInvalidClientTokenErrorsMaxAttemptCount);
					throw new CiscoApiException("Timeout during authentication - gave up trying to get token after an invalid_client error.", ex);
				}
			// Standard catch
			catch (Exception ex) when (
				ex is TaskCanceledException
				// 2025-09-26 If Options.RetryInvalidClientTokenErrors is false, retry invalid_client errors using the normal query retry defaults.
				|| (ex is SecurityException && ex.Message.StartsWith("invalid_client")))
			{
				if (++attemptCount < Options.MaxAttemptCount)
				{
					_logger.LogWarning("GetAccessTokenAsync(): Attempt {AttemptCount}/{MaxAttemptCount} failed, retrying...",
						attemptCount,
						Options.MaxAttemptCount
					);

					await Task.Delay(Options.RetryDelay, cancellationToken)
						.ConfigureAwait(false);

					continue;
				}

				_logger.LogError(
					ex,
					"GetAccessTokenAsync(): {Message} after {MaxAttemptCount} attempts.",
					ex.Message,
					Options.MaxAttemptCount);
				throw new CiscoApiException("Timeout during authentication.", ex);
			}

			var contents = await httpResponseMessage
				.Content
				.ReadAsStringAsync(cancellationToken)
				.ConfigureAwait(false);

			var accessTokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(contents)
				?? throw new FormatException("Unable to deserialize access token response");
			if (accessTokenResponse.ErrorDescription != null || accessTokenResponse.Error != null)
			{
				_logger.LogDebug("Authentication failed.");

				// If response not yet logged and OnErrorEnsureRequestResponseHeadersLogged is set, then log as error
				if (!_logger.IsEnabled(LevelToLogAt) && Options.OnErrorEnsureRequestResponseHeadersLogged)
				{
					await LogResponseHeaders(currentCredentialCurrentIndex, httpResponseMessage, true).ConfigureAwait(false);
				}

				throw new SecurityException($"{accessTokenResponse.Error}: {accessTokenResponse.ErrorDescription}");
			}

			_logger.LogDebug("Authentication succeeded.");

			// Defaulting to just under 1 hour if not available in response
			var expireInSeconds = accessTokenResponse.ExpiresInSeconds ?? 3540;

			// If there is an expiry, try to take 1 minute off it unless it is already less than a minute
			// This is to resolve corner cases where the expiry took a few seconds to be returned, and so the calculated expiry time
			// leaves a small window for the token to have expired before the next request, allowing a query to fail.
			if (accessTokenResponse.ExpiresInSeconds - 60 > 0)
			{
				expireInSeconds -= 60;
				//_logger.LogDebug("The expiry has been reduced further by a safety margin of 1 minute, to deal with any delay in the token response being returned.");
			}
			_logger.LogDebug($"Access token should expire in {expireInSeconds} seconds.");

			// Store the expiry timestamp
			CiscoUmbrellaCredentials.ElementAt(currentCredentialCurrentIndex).Value.AccessTokenExpiryDateTimeOffset = DateTimeOffset.UtcNow.AddSeconds(expireInSeconds);

			_logger.LogDebug(
				"The access token '{AccessToken}' expiry date time is '{ExpiryDateTimeUtc}'",
				accessTokenResponse.AccessToken!,
				CiscoUmbrellaCredentials.ElementAt(currentCredentialCurrentIndex).Value.AccessTokenExpiryDateTimeOffset
			);

			return accessTokenResponse.AccessToken!;
		}
	}

	protected override async Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		// Once CurrentCredentialCurrentIndex is set, it is copied to a local variable so that all refs
		// to it stay the same for the duration of the method, in case another query does a SendAsync() at the same time and changed it, which
		// probably write new values to the wrong token if I use CurrentCredentialCurrentIndex everywhere, (although it might not be a problem if the token is still valid).
		// So far I'm not using this in parallel in Nucleus.

		// Use the next credential in the list
		if (IsFirstQuery)
		{
			// Leave the index at 0 for first run
			IsFirstQuery = false;
		}
		else
		{
			CurrentCredentialCurrentIndex = (CurrentCredentialCurrentIndex + 1) % CiscoUmbrellaCredentials.Count;
		}
		var currentCredentialCurrentIndex = CurrentCredentialCurrentIndex;
		var currentCredentialsInstance = CiscoUmbrellaCredentials.ElementAt(currentCredentialCurrentIndex).Value;
		// There might be an authentication token already that is about to expire so check first.
		if (currentCredentialsInstance.AccessTokenExpiryDateTimeOffset is not null
			&& currentCredentialsInstance.AccessTokenExpiryDateTimeOffset <= DateTimeOffset.UtcNow)
		{
			_logger.LogDebug("SendAsync(): The access token expiry date time ('{AccessTokenExpiryDateTimeOffset}') has expired - getting a new token...", currentCredentialsInstance.AccessTokenExpiryDateTimeOffset);
			currentCredentialsInstance.AccessToken = await GetAccessTokenAsync(currentCredentialCurrentIndex, cancellationToken)
				.ConfigureAwait(false);
			currentCredentialsInstance.AuthenticationHeaderValue = new AuthenticationHeaderValue("Bearer", currentCredentialsInstance.AccessToken);
		}

		if (currentCredentialsInstance.AuthenticationHeaderValue is null)
		{
			currentCredentialsInstance.AccessToken = await GetAccessTokenAsync(currentCredentialCurrentIndex, cancellationToken)
				.ConfigureAwait(false);
			currentCredentialsInstance.AuthenticationHeaderValue = new AuthenticationHeaderValue("Bearer", currentCredentialsInstance.AccessToken);
		}

		//_logger.LogDebug($"SendAsync(): About to send query. The access token expiry date time is '{_accessTokenExpiryDateTimeOffset}'.");

		request.Headers.Authorization = currentCredentialsInstance.AuthenticationHeaderValue;
		request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

		// Only do diagnostic logging if we're at the level we want to enable for as this is more efficient
		if (_logger.IsEnabled(LevelToLogAt))
		{
			await LogRequestHeaders(currentCredentialCurrentIndex, request).ConfigureAwait(false);
		}

		if (Options.UserAgent is not null)
		{
			request.Headers.Add("User-Agent", Options.UserAgent);
		}

		var attemptCount = 0;
		while (true)
		{
			HttpResponseMessage httpResponseMessage;
			try
			{
				httpResponseMessage = await base
					.SendAsync(request, cancellationToken)
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

					await Task.Delay(Options.RetryDelay, cancellationToken)
						.ConfigureAwait(false);

					continue;
				}

				// Retries not enabled or retries exhausted, so log as error

				// If request headers haven't been logged so far, but OnErrorEnsureRequestResponseHeadersShown is true,
				// then log them. Avoids need for verbose logging of all queries.
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

			// Only do diagnostic logging if we're at the level we want to enable for as this is more efficient
			if (_logger.IsEnabled(LevelToLogAt))
			{
				await LogResponseHeaders(currentCredentialCurrentIndex, httpResponseMessage).ConfigureAwait(false);
			}

			// Make response stream content accessible in debug
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

				switch (httpResponseMessage.StatusCode)
				{
					case HttpStatusCode.TooManyRequests:
						if (++attemptCount < Options.MaxAttemptCount)
						{
							// Set the retry based on 'retry-after' header, or use Options.RetryDelay
							var headers = httpResponseMessage.Headers;
							var retryAfter = headers.RetryAfter?.Delta;
							var retryDelay = retryAfter is not null
								? retryAfter.Value
								: Options.RetryDelay;

							_logger.LogWarning(
								"Attempt {AttemptCount}/{MaxAttemptCount} failed due to a 429, retrying in {x} seconds...",
								attemptCount,
								Options.MaxAttemptCount,
								retryDelay);

							await Task.Delay(retryDelay, cancellationToken)
								.ConfigureAwait(false);

							continue;
						}

						break;
					case HttpStatusCode.BadGateway:
					case HttpStatusCode.GatewayTimeout:
					case HttpStatusCode.InternalServerError:
					case HttpStatusCode.RequestTimeout:
					case HttpStatusCode.ServiceUnavailable:
					// Adding this because it seems that Cisco can return Unauthorized for no reason
					case HttpStatusCode.Unauthorized:
						if (++attemptCount < Options.MaxAttemptCount)
						{
							if (message.Contains("Developer Inactive"))
							{
								// Cisco API can return an incorrect 403 if their load balancer hasn't got access to
								// the latest state of a token. Request a new token so that follow-up retry probably works.
								_logger.LogDebug($"SendAsync(): Response content was Developer Inactive - could be a bad API response, requesting a new token.");
								currentCredentialsInstance.AccessToken = await GetAccessTokenAsync(currentCredentialCurrentIndex, cancellationToken)
									.ConfigureAwait(false);
								currentCredentialsInstance.AuthenticationHeaderValue = new AuthenticationHeaderValue("Bearer", currentCredentialsInstance.AccessToken);
								request.Headers.Authorization = currentCredentialsInstance.AuthenticationHeaderValue;
							}

							_logger.LogWarning(
								"Attempt {AttemptCount}/{MaxAttemptCount} failed, retrying...",
								attemptCount,
								Options.MaxAttemptCount);

							await Task.Delay(Options.RetryDelay, cancellationToken)
								.ConfigureAwait(false);

							continue;
						}

						break;
				}

				// Retries not enabled or retries exhausted, so log as error

				// If request/response headers haven't been logged so far, but OnErrorEnsureRequestResponseHeadersShown is true,
				// then log them both. Avoids need for verbose logging of all queries.
				if (!_logger.IsEnabled(LevelToLogAt) && Options.OnErrorEnsureRequestResponseHeadersLogged)
				{
					await LogRequestHeaders(currentCredentialCurrentIndex, request, true).ConfigureAwait(false);
					await LogResponseHeaders(currentCredentialCurrentIndex, httpResponseMessage, true).ConfigureAwait(false);
				}

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


	public abstract HttpClient GetHttpClient(int currentCredentialCurrentIndex);

	public abstract StringContent GetAuthBody();
}