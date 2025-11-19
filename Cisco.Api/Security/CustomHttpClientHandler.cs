using Cisco.Api.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Security;

internal abstract class CustomHttpClientHandler(
	Uri authenticationUri,
	CiscoClientOptions options,
	ILogger logger) : HttpClientHandler
{
	private AuthenticationHeaderValue? _authenticationHeaderValue;
	private readonly ILogger _logger = logger;
	private const LogLevel LevelToLogAt = LogLevel.Trace;
	private string? _accessToken = options.Token;
	private DateTimeOffset? _accessTokenExpiryDateTimeOffset;
	private readonly bool _useJsonContentType = options.UseJsonContentType;

	protected Uri AuthUri { get; } = authenticationUri;
	protected CiscoClientOptions Options { get; } = options;

	private async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
	{
		// If the user mistakenly sets Options.ClientCredentials, but isn't querying the Umbrella fast client, they'll have been able
		// to reach here, so double check that the appropriate ClientId and ClientSecret are set.
		if (Options.ClientId is null)
		{
			throw new ArgumentException("Options ClientId must be set", nameof(Options));
		}
		if (Options.ClientSecret is null)
		{
			throw new ArgumentException("Options ClientSecret must be set", nameof(Options));
		}

		_logger.LogDebug("Authenticating...");

		var attemptCount = 0;

		while (true)
		{
			using var httpClient = GetHttpClient();

			HttpResponseMessage httpResponseMessage;
			try
			{
				httpResponseMessage = await httpClient
					.PostAsync(string.Empty, GetAuthBody(), cancellationToken)
					.ConfigureAwait(false);

				_logger.LogTrace("{HttpResponseMessage}", httpResponseMessage);
			}
			// Catch network-related exceptions that might be transient and worth retrying
			catch (Exception ex) when (
				ex is TaskCanceledException
				|| ex is HttpRequestException
				|| ex is TimeoutException
				|| ex is SocketException
				|| (ex is IOException ioEx && ioEx.InnerException is SocketException))
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
				
				// Retries not enabled or retries exhausted, so log as error
				throw new CiscoApiException("Timeout or transient network failure during authentication.", ex);
			}

			var contents = await httpResponseMessage
				.Content
				.ReadAsStringAsync(cancellationToken)
				.ConfigureAwait(false);

			var accessTokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(contents)
				?? throw new FormatException("Unable to deserialize access token response");

			// Handle error responses (including invalid_client) BEFORE throwing
			if (accessTokenResponse.Error is not null)
			{
				var error = accessTokenResponse.Error;
				var description = accessTokenResponse.ErrorDescription;
				var combinedMessage = description is { Length: > 0 }
					? $"{error}: {description}"
					: error;

				_logger.LogDebug("Authentication failed. Error={Error} Description={Description}", error, description);

				if (!_logger.IsEnabled(LevelToLogAt) && Options.OnErrorEnsureRequestResponseHeadersLogged)
				{
					await LogResponseHeaders(httpResponseMessage, true).ConfigureAwait(false);
				}

				var isInvalidClient = error.Equals("invalid_client", StringComparison.OrdinalIgnoreCase);

				// Priority: dedicated invalid_client retry settings if enabled
				if (isInvalidClient && Options.RetryInvalidClientTokenErrors)
				{
					if (++attemptCount < Options.RetryInvalidClientTokenErrorsMaxAttemptCount)
					{
						_logger.LogWarning("GetAccessTokenAsync(): invalid_client ({AttemptCount}/{MaxAttemptCount}) – retrying after {Delay}s...",
							attemptCount,
							Options.RetryInvalidClientTokenErrorsMaxAttemptCount,
							Options.RetryInvalidClientTokenErrorsRetryDelay.TotalSeconds);

						await Task.Delay(Options.RetryInvalidClientTokenErrorsRetryDelay, cancellationToken).ConfigureAwait(false);
						continue;
					}

					_logger.LogError("GetAccessTokenAsync(): invalid_client exhausted after {MaxAttemptCount} attempts.",
						Options.RetryInvalidClientTokenErrorsMaxAttemptCount);
					throw new CiscoApiException("Timeout during authentication - gave up trying to get token after repeated invalid_client errors.");
				}

				// Fallback: treat invalid_client as part of normal retry window if special retry disabled
				if (isInvalidClient && ++attemptCount < Options.MaxAttemptCount)
				{
					_logger.LogWarning("GetAccessTokenAsync(): invalid_client ({AttemptCount}/{MaxAttemptCount}) using standard retry settings, retrying after {Delay}s...",
						attemptCount,
						Options.MaxAttemptCount,
						Options.RetryDelay.TotalSeconds);
					await Task.Delay(Options.RetryDelay, cancellationToken).ConfigureAwait(false);
					continue;
				}

				// Non-retriable (or retries exhausted)
				throw new SecurityException(combinedMessage);
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
			_accessTokenExpiryDateTimeOffset = DateTimeOffset.UtcNow.AddSeconds(expireInSeconds);

			_logger.LogDebug(
				"The access token '{AccessToken}' expiry date time is '{ExpiryDateTimeUtc}'",
				accessTokenResponse.AccessToken!,
				_accessTokenExpiryDateTimeOffset
			);

			return accessTokenResponse.AccessToken!;
		}
	}

	protected override async Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken)
	{
		// There might be an authentication token already that is about to expire so check first.
		if (_accessTokenExpiryDateTimeOffset is not null && _accessTokenExpiryDateTimeOffset <= DateTimeOffset.UtcNow)
		{
			_logger.LogDebug("SendAsync(): The access token expiry date time ('{AccessTokenExpiryDateTimeOffset}') has expired - getting a new token...", _accessTokenExpiryDateTimeOffset);
			_accessToken = await GetAccessTokenAsync(cancellationToken)
				.ConfigureAwait(false);
			_authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", _accessToken);
		}

		if (_authenticationHeaderValue is null)
		{
			_accessToken = await GetAccessTokenAsync(cancellationToken)
				.ConfigureAwait(false);
			_authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", _accessToken);
		}

		//_logger.LogDebug($"SendAsync(): About to send query. The access token expiry date time is '{_accessTokenExpiryDateTimeOffset}'.");

		request.Headers.Authorization = _authenticationHeaderValue;
		if (_useJsonContentType)
		{
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			// Convert the request content to JSON if necessary
			string? originalContent = string.Empty;
			if (request?.Content != null)
			{
				originalContent = await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
			}

			// Some of the Cisco APIs require we set the header regardless of whether there is content or not
			// Hence sending even an empty JSON object if there is no content for GETs.
			request.Content = new StringContent(originalContent, Encoding.UTF8, "application/json");
		}
		else
		{
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
		}

		// Only do diagnostic logging if we're at the level we want to enable for as this is more efficient
		if (_logger.IsEnabled(LevelToLogAt))
		{
			await LogRequestHeaders(request).ConfigureAwait(false);
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
					await LogRequestHeaders(request, true).ConfigureAwait(false);
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
				await LogResponseHeaders(httpResponseMessage).ConfigureAwait(false);
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
							// TODO Add retry-after header support

							_logger.LogWarning(
								"Attempt {AttemptCount}/{MaxAttemptCount} failed due to a 429, retrying in {x} seconds...",
								attemptCount,
								Options.MaxAttemptCount,
								Options.RetryDelay);

							await Task.Delay(Options.RetryDelay, cancellationToken)
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
								_accessToken = await GetAccessTokenAsync(cancellationToken)
									.ConfigureAwait(false);
								_authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", _accessToken);
								request.Headers.Authorization = _authenticationHeaderValue;
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
					await LogRequestHeaders(request, true).ConfigureAwait(false);
					await LogResponseHeaders(httpResponseMessage, true).ConfigureAwait(false);
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

	private async Task LogRequestHeaders(HttpRequestMessage request, bool logAsError = false)
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
				"RequestContent\r\n{RequestContext}",
				content
			);
		}
	}

	private async Task LogResponseHeaders(HttpResponseMessage httpResponseMessage, bool logAsError = false)
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
				"ResponseContent\r\n{ResponseContent}",
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


	public abstract HttpClient GetHttpClient();

	public abstract StringContent GetAuthBody();
}