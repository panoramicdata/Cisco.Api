﻿using Cisco.Api.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Security;

internal class UmbrellaHttpClientHandler(
	Uri authenticationUri,
	CiscoClientOptions options,
	ILogger logger) : HttpClientHandler
{
	private AuthenticationHeaderValue? _authenticationHeaderValue;
	private readonly ILogger _logger = logger;
	private const LogLevel LevelToLogAt = LogLevel.Trace;
	private readonly Uri _authUri = authenticationUri;
	private readonly CiscoClientOptions _options = options;
	private string? _accessToken = options.Token;
	private DateTimeOffset? _accessTokenExpiryDateTimeOffset;

	private async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
	{
		_logger.LogDebug("Authenticating...");

		var attemptCount = 0;
		while (true)
		{
			using var httpClient = new HttpClient
			{
				BaseAddress = _authUri,
				Timeout = TimeSpan.FromSeconds(15)
			};
			var base64EncodedClientIdAndSecret = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_options.ClientId}:{_options.ClientSecret}"));
			httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {base64EncodedClientIdAndSecret}");
			var stringContent = new StringContent(
				$"grant_type=client_credentials",
				Encoding.UTF8,
				"application/x-www-form-urlencoded");

			HttpResponseMessage httpResponseMessage;
			try
			{
				httpResponseMessage = await httpClient
					.PostAsync(string.Empty, stringContent, cancellationToken)
					.ConfigureAwait(false);

				_logger.LogTrace("{HttpResponseMessage}", httpResponseMessage);
			}
			catch (TaskCanceledException ex)
			{
				if (++attemptCount < _options.MaxAttemptCount)
				{
					_logger.LogWarning("GetAccessTokenAsync(): Attempt {AttemptCount}/{MaxAttemptCount} failed, retrying...",
						attemptCount,
						_options.MaxAttemptCount
					);

					await Task.Delay(_options.RetryDelay, cancellationToken)
						.ConfigureAwait(false);

					continue;
				}

				_logger.LogError(
					ex,
					"GetAccessTokenAsync(): {Message} after {MaxAttemptCount} attempts.",
					ex.Message,
					_options.MaxAttemptCount);
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
				if (!_logger.IsEnabled(LevelToLogAt) && _options.OnErrorEnsureRequestResponseHeadersLogged)
				{
					await LogResponseHeaders(httpResponseMessage, true).ConfigureAwait(false);
				}

				throw new SecurityException($"{accessTokenResponse.Error}: {accessTokenResponse.ErrorDescription}");
			}

			_logger.LogDebug("Authentication succeeded.");

			var expireInSeconds = accessTokenResponse.ExpiresInSeconds;

			/*
                _logger.LogDebug($"Access token will expire in {expireInSeconds} seconds.");
                // If there is an expiry, try to take 1 minute off it unless it is already less than a minute
                if (accessTokenResponse.ExpiresInSeconds is not null && accessTokenResponse.ExpiresInSeconds - 60 > 0)
                {
                    expireInSeconds -= 60;
                    //_logger.LogDebug("Since expiry > 60 seconds, it has been reduced further by 1 minute to give time to trigger refresh of token.");
                }
                // If not yet set, set expiry to default timeout of the 1 hour max limit, minus a minute.
                if (expireInSeconds is null)
                {
                    //_logger.LogDebug("The access token 'ExpiresInSeconds' was null, setting to 3540 seconds.");
                    expireInSeconds = 3540;
                }
                */

			// Set expiry, defaulting to 1 hour if not set
			_accessTokenExpiryDateTimeOffset = DateTimeOffset.UtcNow.AddSeconds(expireInSeconds ?? 3600);
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
		request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

		// Only do diagnostic logging if we're at the level we want to enable for as this is more efficient
		if (_logger.IsEnabled(LevelToLogAt))
		{
			await LogRequestHeaders(request).ConfigureAwait(false);
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
				if (++attemptCount < _options.MaxAttemptCount)
				{
					_logger.LogWarning(
						"Attempt {AttemptCount}/{MaxAttemptCount} failed, retrying...",
						attemptCount,
						_options.MaxAttemptCount);

					await Task.Delay(_options.RetryDelay, cancellationToken)
						.ConfigureAwait(false);

					continue;
				}

				// Retries not enabled or retries exhausted, so log as error

				// If request headers haven't been logged so far, but OnErrorEnsureRequestResponseHeadersShown is true,
				// then log them. Avoids need for verbose logging of all queries.
				if (!_logger.IsEnabled(LevelToLogAt) && _options.OnErrorEnsureRequestResponseHeadersLogged)
				{
					await LogRequestHeaders(request, true).ConfigureAwait(false);
				}

				_logger.LogError(
					ex, "{Message} after {MaxAttemptCount} attempts.",
					ex.Message,
					_options.MaxAttemptCount
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
					case HttpStatusCode.BadGateway:
					case HttpStatusCode.GatewayTimeout:
					case HttpStatusCode.InternalServerError:
					case HttpStatusCode.RequestTimeout:
					case HttpStatusCode.ServiceUnavailable:
					// Adding this because it seems that Cisco can return Unauthorized for no reason
					case HttpStatusCode.Unauthorized:
						if (++attemptCount < _options.MaxAttemptCount)
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
								_options.MaxAttemptCount);

							await Task.Delay(_options.RetryDelay, cancellationToken)
								.ConfigureAwait(false);

							continue;
						}

						break;
				}

				// Retries not enabled or retries exhausted, so log as error

				// If request/response headers haven't been logged so far, but OnErrorEnsureRequestResponseHeadersShown is true,
				// then log them both. Avoids need for verbose logging of all queries.
				if (!_logger.IsEnabled(LevelToLogAt) && _options.OnErrorEnsureRequestResponseHeadersLogged)
				{
					await LogRequestHeaders(request, true).ConfigureAwait(false);
					await LogResponseHeaders(httpResponseMessage, true).ConfigureAwait(false);
				}

				_logger.LogError(
					"{Message} after {MaxAttemptCount} attempts.",
					message,
					_options.MaxAttemptCount
				);

				throw new CiscoApiException(httpResponseMessage);
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
}
