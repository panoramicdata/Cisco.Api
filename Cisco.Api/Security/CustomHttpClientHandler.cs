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

				var accessTokenResponse = await DeserializeTokenResponseAsync(httpResponseMessage, cancellationToken).ConfigureAwait(false);

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

	private static async Task<AccessTokenResponse> DeserializeTokenResponseAsync(HttpResponseMessage httpResponseMessage, CancellationToken cancellationToken)
	{
		var contents = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
		return JsonConvert.DeserializeObject<AccessTokenResponse>(contents)
			?? throw new FormatException("Unable to deserialize access token response");
	}

	private async Task<bool> HandleAuthErrorAsync(
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
			await LogResponseHeaders(httpResponseMessage, true).ConfigureAwait(false);
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

			throw await CreateResponseExceptionAsync(request, attempt.Response, message, cancellationToken).ConfigureAwait(false);
		}
	}

	private static Task<byte[]?> ReadRequestBodyAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		=> request.Content is null
			? Task.FromResult<byte[]?>(null)
			: ReadContentAsync(request.Content, cancellationToken);

	private static async Task<byte[]?> ReadContentAsync(HttpContent content, CancellationToken cancellationToken)
		=> await content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);

	private async Task<(HttpResponseMessage? Response, int AttemptCount)> SendAttemptAsync(
		HttpRequestMessage attemptRequest,
		HttpRequestMessage originalRequest,
		int attemptCount,
		CancellationToken cancellationToken)
	{
		try
		{
			return (await base.SendAsync(attemptRequest, cancellationToken).ConfigureAwait(false), attemptCount);
		}
		catch (Exception ex)
		{
			attemptCount++;
			if (attemptCount < Options.MaxAttemptCount)
			{
				_logger.LogWarning("Attempt {AttemptCount}/{MaxAttemptCount} failed, retrying...", attemptCount, Options.MaxAttemptCount);
				await Task.Delay(Options.RetryDelay, CancellationToken.None).ConfigureAwait(false);
				return (null, attemptCount);
			}

			if (!_logger.IsEnabled(LevelToLogAt) && Options.OnErrorEnsureRequestResponseHeadersLogged)
			{
				await LogRequestHeaders(originalRequest, true).ConfigureAwait(false);
			}

			_logger.LogError(ex, "{Message} after {MaxAttemptCount} attempts.", ex.Message, Options.MaxAttemptCount);
			throw new CiscoApiException(ex.Message, ex);
		}
	}

	private async Task LogResponseHeadersIfNeeded(HttpResponseMessage response)
	{
		if (_logger.IsEnabled(LevelToLogAt))
		{
			await LogResponseHeaders(response).ConfigureAwait(false);
		}
	}

	private async Task<CiscoApiException> CreateResponseExceptionAsync(
		HttpRequestMessage request,
		HttpResponseMessage response,
		string message,
		CancellationToken cancellationToken)
	{
		await LogErrorHeadersIfNeeded(request, response).ConfigureAwait(false);
		_logger.LogError("{Message} after {MaxAttemptCount} attempts.", message, Options.MaxAttemptCount);
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

	private async Task<bool> HandleRetriableStatusCodeAsync(
		HttpResponseMessage httpResponseMessage,
		string message,
		HttpRequestMessage request,
		int[] attemptCount,
		CancellationToken cancellationToken)
	{
		if (httpResponseMessage.StatusCode == HttpStatusCode.TooManyRequests)
		{
			return await RetryAfterRateLimitAsync(attemptCount).ConfigureAwait(false);
		}

		if (!IsTransientStatusCode(httpResponseMessage.StatusCode))
		{
			return false;
		}

		return await RetryAfterTransientFailureAsync(message, request, attemptCount, cancellationToken).ConfigureAwait(false);
	}

	private async Task<bool> RetryAfterRateLimitAsync(int[] attemptCount)
	{
		if (++attemptCount[0] >= Options.MaxAttemptCount)
		{
			return false;
		}

		_logger.LogWarning(
			"Attempt {AttemptCount}/{MaxAttemptCount} failed due to a 429, retrying in {RetryDelay}...",
			attemptCount[0], Options.MaxAttemptCount, Options.RetryDelay);
		await Task.Delay(Options.RetryDelay, CancellationToken.None).ConfigureAwait(false);
		return true;
	}

	private async Task<bool> RetryAfterTransientFailureAsync(
		string message,
		HttpRequestMessage request,
		int[] attemptCount,
		CancellationToken cancellationToken)
	{
		if (++attemptCount[0] >= Options.MaxAttemptCount)
		{
			return false;
		}

		if (message.Contains("Developer Inactive", StringComparison.Ordinal))
		{
			_logger.LogDebug("SendAsync(): Response content was Developer Inactive - could be a bad API response, requesting a new token.");
			var refreshedToken = await GetAccessTokenAsync(cancellationToken).ConfigureAwait(false);
			_authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", refreshedToken);
			request.Headers.Authorization = _authenticationHeaderValue;
		}

		_logger.LogWarning("Attempt {AttemptCount}/{MaxAttemptCount} failed, retrying...", attemptCount[0], Options.MaxAttemptCount);
		await Task.Delay(Options.RetryDelay, CancellationToken.None).ConfigureAwait(false);
		return true;
	}

	private static bool IsTransientStatusCode(HttpStatusCode statusCode)
		=> statusCode is HttpStatusCode.BadGateway
			or HttpStatusCode.GatewayTimeout
			or HttpStatusCode.InternalServerError
			or HttpStatusCode.RequestTimeout
			or HttpStatusCode.ServiceUnavailable
			or HttpStatusCode.Unauthorized;

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

	public abstract HttpClient GetHttpClient();

	public abstract StringContent GetAuthBody();
}
