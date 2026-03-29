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
	private DateTimeOffset? _accessTokenExpiryDateTimeOffset;
	private readonly bool _useJsonContentType = options.UseJsonContentType;

	protected Uri AuthUri { get; } = authenticationUri;
	protected CiscoClientOptions Options { get; } = options;

	private async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
	{
		ValidateClientCredentials();

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
			catch (Exception ex) when (IsTransientException(ex))
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

				throw new CiscoApiException("Timeout or transient network failure during authentication.", ex);
			}

			var contents = await httpResponseMessage
				.Content
				.ReadAsStringAsync(cancellationToken)
				.ConfigureAwait(false);

			var accessTokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(contents)
				?? throw new FormatException("Unable to deserialize access token response");

			if (accessTokenResponse.Error is not null)
			{
				var attemptCountRef = new[] { attemptCount };
				var shouldContinue = await HandleAuthErrorAsync(accessTokenResponse, httpResponseMessage, attemptCountRef, cancellationToken)
					.ConfigureAwait(false);
				attemptCount = attemptCountRef[0];
				if (shouldContinue)
				{
					continue;
				}
			}

			return StoreAndReturnAccessToken(accessTokenResponse);
		}
	}

	private void ValidateClientCredentials()
	{
		if (Options.ClientId is null)
		{
			throw new SecurityException("Options ClientId must be set");
		}

		if (Options.ClientSecret is null)
		{
			throw new SecurityException("Options ClientSecret must be set");
		}
	}

	private static bool IsTransientException(Exception ex)
		=> ex is TaskCanceledException
			or HttpRequestException
			or TimeoutException
			or SocketException
			|| (ex is IOException ioEx && ioEx.InnerException is SocketException);

	private async Task<bool> HandleAuthErrorAsync(
		AccessTokenResponse accessTokenResponse,
		HttpResponseMessage httpResponseMessage,
		int[] attemptCount,
		CancellationToken cancellationToken)
	{
		var error = accessTokenResponse.Error!;
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

		if (isInvalidClient && Options.RetryInvalidClientTokenErrors)
		{
			if (++attemptCount[0] < Options.RetryInvalidClientTokenErrorsMaxAttemptCount)
			{
				_logger.LogWarning("GetAccessTokenAsync(): invalid_client ({AttemptCount}/{MaxAttemptCount}) – retrying after {Delay}s...",
					attemptCount,
					Options.RetryInvalidClientTokenErrorsMaxAttemptCount,
					Options.RetryInvalidClientTokenErrorsRetryDelay.TotalSeconds);

				await Task.Delay(Options.RetryInvalidClientTokenErrorsRetryDelay, cancellationToken).ConfigureAwait(false);
				return true;
			}

			_logger.LogError("GetAccessTokenAsync(): invalid_client exhausted after {MaxAttemptCount} attempts.",
				Options.RetryInvalidClientTokenErrorsMaxAttemptCount);
			throw new CiscoApiException("Timeout during authentication - gave up trying to get token after repeated invalid_client errors.");
		}

		if (isInvalidClient && ++attemptCount[0] < Options.MaxAttemptCount)
		{
			_logger.LogWarning("GetAccessTokenAsync(): invalid_client ({AttemptCount}/{MaxAttemptCount}) using standard retry settings, retrying after {Delay}s...",
				attemptCount,
				Options.MaxAttemptCount,
				Options.RetryDelay.TotalSeconds);
			await Task.Delay(Options.RetryDelay, cancellationToken).ConfigureAwait(false);
			return true;
		}

		throw new SecurityException(combinedMessage);
	}

	private string StoreAndReturnAccessToken(AccessTokenResponse accessTokenResponse)
	{
		_logger.LogDebug("Authentication succeeded.");

		var expireInSeconds = accessTokenResponse.ExpiresInSeconds ?? 3540;

		if (accessTokenResponse.ExpiresInSeconds - 60 > 0)
		{
			expireInSeconds -= 60;
		}

		_logger.LogDebug("Access token should expire in {ExpireInSeconds} seconds.", expireInSeconds);

		_accessTokenExpiryDateTimeOffset = DateTimeOffset.UtcNow.AddSeconds(expireInSeconds);

		_logger.LogDebug(
			"The access token '{AccessToken}' expiry date time is '{ExpiryDateTimeUtc}'",
			accessTokenResponse.AccessToken!,
			_accessTokenExpiryDateTimeOffset
		);

		return accessTokenResponse.AccessToken!;
	}

	protected override async Task<HttpResponseMessage> SendAsync(
		HttpRequestMessage request,
		CancellationToken cancellationToken)
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

			if (_logger.IsEnabled(LevelToLogAt))
			{
				await LogResponseHeaders(httpResponseMessage).ConfigureAwait(false);
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
				if (await HandleRetriableStatusCodeAsync(httpResponseMessage, message, request, attemptCountRef, cancellationToken).ConfigureAwait(false))
				{
					attemptCount = attemptCountRef[0];
					continue;
				}

				attemptCount = attemptCountRef[0];

				await LogErrorHeadersIfNeeded(request, httpResponseMessage).ConfigureAwait(false);

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

	private async Task<bool> HandleRetriableStatusCodeAsync(
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
					_logger.LogWarning(
						"Attempt {AttemptCount}/{MaxAttemptCount} failed due to a 429, retrying in {x} seconds...",
						attemptCount[0],
						Options.MaxAttemptCount,
						Options.RetryDelay);

					await Task.Delay(Options.RetryDelay, cancellationToken).ConfigureAwait(false);
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
						var refreshedToken = await GetAccessTokenAsync(cancellationToken).ConfigureAwait(false);
						_authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", refreshedToken);
						request.Headers.Authorization = _authenticationHeaderValue;
					}

					_logger.LogWarning(
						"Attempt {AttemptCount}/{MaxAttemptCount} failed, retrying...",
						attemptCount[0],
						Options.MaxAttemptCount);

					await Task.Delay(Options.RetryDelay, cancellationToken).ConfigureAwait(false);
					return true;
				}

				break;
		}

		return false;
	}

	private async Task LogErrorHeadersIfNeeded(HttpRequestMessage request, HttpResponseMessage httpResponseMessage)
	{
		if (!_logger.IsEnabled(LevelToLogAt) && Options.OnErrorEnsureRequestResponseHeadersLogged)
		{
			await LogRequestHeaders(request, true).ConfigureAwait(false);
			await LogResponseHeaders(httpResponseMessage, true).ConfigureAwait(false);
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