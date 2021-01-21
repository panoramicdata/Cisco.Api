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
            if (_accessToken != null)
            {
                return _accessToken;
            }

            _logger.LogDebug("Authenticating...");
            using var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_url)
            };
            var stringContent = new StringContent(
                $"client_id={_options.ClientId}&grant_type=client_credentials&client_secret={_options.ClientSecret}",
                Encoding.UTF8,
                "application/x-www-form-urlencoded");
            var response = await httpClient
                .PostAsync(_endpoint, stringContent, cancellationToken)
                .ConfigureAwait(false);
            _logger.LogTrace($"{response}");
            var contents = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var accessTokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(contents);
            if (accessTokenResponse.ErrorDescription != null || accessTokenResponse.Error != null)
            {
                _logger.LogDebug("Authentication failed.");
                throw new SecurityException($"{accessTokenResponse.Error}: {accessTokenResponse.ErrorDescription}");
            }
            _logger.LogDebug("Authentication succeeded.");
            return accessTokenResponse.AccessToken!;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (_authenticationHeaderValue is null)
            {
                _accessToken ??= await GetAccessTokenAsync(cancellationToken)
                    .ConfigureAwait(false);
                _authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", _accessToken);
            }

            request.Headers.Authorization = _authenticationHeaderValue;
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

            // Only do diagnostic logging if we're at the level we want to enable for as this is more efficient
            if (_logger.IsEnabled(LevelToLogAt))
            {
                _logger.Log(LevelToLogAt, $"Request\r\n{request}");
                if (request.Content != null)
                {
                    _logger.Log(LevelToLogAt, "RequestContent\r\n" + await request.Content.ReadAsStringAsync().ConfigureAwait(false));
                }
            }

            var attemptCount = 0;
            while (true)
            {
                var httpResponseMessage = await base
                    .SendAsync(request, cancellationToken)
                    .ConfigureAwait(false);

                // Only do diagnostic logging if we're at the level we want to enable for as this is more efficient
                if (_logger.IsEnabled(LevelToLogAt))
                {
                    _logger.Log(LevelToLogAt, $"Response\r\n{httpResponseMessage}");
                    if (httpResponseMessage.Content != null)
                    {
                        _logger.Log(LevelToLogAt, "ResponseContent\r\n" + await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false));
                    }
                }

                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    var statusCode = httpResponseMessage.StatusCode;
                    var content = httpResponseMessage.Content;
                    if (content != null)
                    {
                        var responseBody = await content
                            .ReadAsStringAsync()
                            .ConfigureAwait(false);
                        _logger.LogError($"{statusCode}: {responseBody}");
                    }
                    else
                    {
                        _logger.LogError(statusCode.ToString());
                    }
                    switch (httpResponseMessage.StatusCode)
                    {
                        case HttpStatusCode.BadGateway:
                        case HttpStatusCode.GatewayTimeout:
                        case HttpStatusCode.InternalServerError:
                        case HttpStatusCode.RequestTimeout:
                        case HttpStatusCode.ServiceUnavailable:
                            if (++attemptCount < _options.MaxAttemptCount)
                            {
                                _logger.Log(LevelToLogAt, $"Attempt {attemptCount}/{_options.MaxAttemptCount} failed, retrying...");
                                await Task.Delay(_options.RetryDelay, cancellationToken).ConfigureAwait(false);
                                continue;
                            }
                            break;
                    }
                    throw new CiscoApiException(httpResponseMessage);
                }

                return httpResponseMessage;
            }
        }
    }
}
