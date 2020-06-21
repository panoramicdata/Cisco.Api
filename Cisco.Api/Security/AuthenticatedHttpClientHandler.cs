using Cisco.Api.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
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
		private string? _accessToken;
		private readonly ILogger _logger;
		private readonly string _clientId;
		private readonly string _clientSecret;
		private const LogLevel LevelToLogAt = LogLevel.Trace;
		private readonly string _url;
		private readonly string _endpoint;

		public AuthenticatedHttpClientHandler(
			string url,
			string endpoint,
			string clientId,
			string clientSecret,
			string? accessToken,
			ILogger logger)
		{
			_url = url;
			_endpoint = endpoint;
			_accessToken = accessToken;
			_logger = logger;
			_clientId = clientId;
			_clientSecret = clientSecret;
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
				$"client_id={_clientId}&grant_type=client_credentials&client_secret={_clientSecret}",
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

			// Only do diagnostic logging if we're at the level we want to enable for as this is more efficient
			if (_logger.IsEnabled(LevelToLogAt))
			{
				_logger.Log(LevelToLogAt, $"Request\r\n{request}");
				if (request.Content != null)
				{
					_logger.Log(LevelToLogAt, "RequestContent\r\n" + await request.Content.ReadAsStringAsync().ConfigureAwait(false));
				}
			}

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
				throw new CiscoApiException(httpResponseMessage);
			}

			return httpResponseMessage;
		}
	}
}
