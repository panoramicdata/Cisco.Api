using Cisco.Api.Exceptions;
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

namespace Cisco.Api.Security
{
    internal class AuthenticatedHttpClientHandler : HttpClientHandler
    {
        private AuthenticationHeaderValue? _authenticationHeaderValue;
        private readonly ILogger _logger;
        private const LogLevel LevelToLogAt = LogLevel.Trace;
        private readonly string _url;
        private readonly string _endpoint;
        private readonly CiscoClientOptions _options;
        private string? _accessToken;
        private DateTimeOffset? _accessTokenExpiryDateTimeOffset;

        public AuthenticatedHttpClientHandler(
            string url,
            string endpoint,
            CiscoClientOptions options,
            ILogger logger)
        {
            _url = url;
            _endpoint = endpoint;
            _options = options;
            _accessToken = options.Token;
            _logger = logger;
        }

        private async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Authenticating...");

            var attemptCount = 0;
            while (true)
            {
                using var httpClient = new HttpClient
                {
                    BaseAddress = new Uri(_url),
                    Timeout = TimeSpan.FromSeconds(15)
                };
                var stringContent = new StringContent(
                    $"client_id={_options.ClientId}&grant_type=client_credentials&client_secret={_options.ClientSecret}",
                    Encoding.UTF8,
                    "application/x-www-form-urlencoded");

                HttpResponseMessage httpResponseMessage;
                try
                {
                    httpResponseMessage = await httpClient
                        .PostAsync(_endpoint, stringContent, cancellationToken)
                        .ConfigureAwait(false);

                    _logger.LogTrace($"{httpResponseMessage}");
                }
                catch (TaskCanceledException ex)
                {
                    if (++attemptCount < _options.MaxAttemptCount)
                    {
                        _logger.LogWarning($"Attempt {attemptCount}/{_options.MaxAttemptCount} failed, retrying...");
                        await Task.Delay(_options.RetryDelay, cancellationToken).ConfigureAwait(false);
                        continue;
                    }
                    _logger.LogError(ex, $"{ex.Message} after {_options.MaxAttemptCount} attempts.");
                    throw new CiscoApiException("Timeout during authentication.", ex);
                }

                var contents = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                var accessTokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(contents);
                if (accessTokenResponse.ErrorDescription != null || accessTokenResponse.Error != null)
                {
                    _logger.LogDebug("Authentication failed.");

                    // If reponse not yet logged and OnErrorEnsureRequestResponseHeadersLogged is set, then log as error
                    if (!_logger.IsEnabled(LevelToLogAt) && _options.OnErrorEnsureRequestResponseHeadersLogged)
                    {
                        await LogResponseHeaders(httpResponseMessage, true).ConfigureAwait(false);
                    }
                    throw new SecurityException($"{accessTokenResponse.Error}: {accessTokenResponse.ErrorDescription}");
                }

                _logger.LogDebug("Authentication succeeded.");

                var expireInSeconds = accessTokenResponse.ExpiresInSeconds;
                _logger.LogDebug($"Access token will expire in {expireInSeconds} seconds.");
                // If there is an expiry, try to take 1 minute off it unless it is already less than a minute
                if (accessTokenResponse.ExpiresInSeconds is not null && accessTokenResponse.ExpiresInSeconds - 60 > 0)
                {
                    expireInSeconds -= 60;
                    _logger.LogDebug("Since expiry > 60 seconds, it has been reduced further by 1 minute to give time to trigger refresh of token.");
                }
                // If not yet set, set expiry to default timeout of the 1 hour max limit, minus a minute.
                if (expireInSeconds is null)
                {
                    _logger.LogDebug("The access token 'ExpiresInSeconds' was null, setting to 3540 seconds.");
                    expireInSeconds = 3540;
                }
                _accessTokenExpiryDateTimeOffset = DateTimeOffset.UtcNow.AddSeconds((double)expireInSeconds);
                _logger.LogDebug($"The access token expiry date time is '{_accessTokenExpiryDateTimeOffset}'");

                return accessTokenResponse.AccessToken!;
            }
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // There might be an auth token already that is about to expire so check first.
            if (_accessTokenExpiryDateTimeOffset is not null && _accessTokenExpiryDateTimeOffset <= DateTimeOffset.UtcNow)
            {
                _logger.LogDebug($"SendAsync(): The access token expiry date time ('{_accessTokenExpiryDateTimeOffset}') has just expired. Getting a new auth token...");
                _accessToken = await GetAccessTokenAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

            if (_authenticationHeaderValue is null)
            {
                _accessToken ??= await GetAccessTokenAsync(cancellationToken)
                    .ConfigureAwait(false);
                _authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", _accessToken);
            }

            _logger.LogDebug($"SendAsync(): About to send query. The access token expiry date time is '{_accessTokenExpiryDateTimeOffset}'.");

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
                        _logger.LogWarning($"Attempt {attemptCount}/{_options.MaxAttemptCount} failed, retrying...");
                        await Task.Delay(_options.RetryDelay, cancellationToken).ConfigureAwait(false);
                        continue;
                    }

                    // Retries not enabled or retries exhausted, so log as error

                    // If request headers haven't been logged so far, but OnErrorEnsureRequestResponseHeadersShown is true,
                    // then log them. Avoids need for verbose logging of all queries.
                    if (!_logger.IsEnabled(LevelToLogAt) && _options.OnErrorEnsureRequestResponseHeadersLogged)
                    {
                        await LogRequestHeaders(request, true).ConfigureAwait(false);
                    }

                    _logger.LogError(ex, $"{ex.Message} after {_options.MaxAttemptCount} attempts.");
                    throw new CiscoApiException(ex.Message, ex);
                }

                // Only do diagnostic logging if we're at the level we want to enable for as this is more efficient
                if (_logger.IsEnabled(LevelToLogAt))
                {
                    await LogResponseHeaders(httpResponseMessage).ConfigureAwait(false);
                }

                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    var statusCode = httpResponseMessage.StatusCode;
                    var content = httpResponseMessage.Content;

                    var message = await GetResponseContent(statusCode, content).ConfigureAwait(false);

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
                                _logger.LogWarning($"Attempt {attemptCount}/{_options.MaxAttemptCount} failed, retrying...");
                                await Task.Delay(_options.RetryDelay, cancellationToken).ConfigureAwait(false);
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

                    _logger.LogError($"{message} after {_options.MaxAttemptCount} attempts.");

                    throw new CiscoApiException(httpResponseMessage);
                }

                return httpResponseMessage;
            }
        }

        private async Task LogRequestHeaders(HttpRequestMessage request, bool logAsError = false)
        {
            // Use logging override if set
            _logger.Log(logAsError ? LogLevel.Error : LevelToLogAt, $"Request\r\n{request}");
            if (request.Content != null)
            {
                _logger.Log(logAsError ? LogLevel.Error : LevelToLogAt, "RequestContent\r\n" + await request.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
        }

        private async Task LogResponseHeaders(HttpResponseMessage httpResponseMessage, bool logAsError = false)
        {
            // Use logging override if set
            _logger.Log(logAsError ? LogLevel.Error : LevelToLogAt, $"Response\r\n{httpResponseMessage}");
            if (httpResponseMessage.Content != null)
            {
                _logger.Log(logAsError ? LogLevel.Error : LevelToLogAt, "ResponseContent\r\n" + await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
        }

        private async Task<string> GetResponseContent(HttpStatusCode statusCode, HttpContent? content)
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
}
