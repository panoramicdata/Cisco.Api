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
		private string? _accessToken;
		private AuthenticationHeaderValue? _authenticationHeaderValue;
		private readonly ILogger _logger;
		private readonly string _clientId;
		private readonly string _clientSecret;
		private const LogLevel LevelToLogAt = LogLevel.Trace;

		public AuthenticatedHttpClientHandler(
			string clientId,
			string clientSecret,
			string? accessToken,
			ILogger logger)
		{
			_accessToken = accessToken;
			_logger = logger;
			_clientId = clientId;
			_clientSecret = clientSecret;
		}

		public async Task AuthenticateAsync(CancellationToken cancellationToken)
		{
			if (_accessToken != null)
			{
				throw new InvalidOperationException("Already authenticated.");
			}

			_logger.LogDebug("Authenticating...");
			using var httpClient = new HttpClient
			{
				BaseAddress = new Uri("https://cloudsso.cisco.com/")
			};
			var stringContent = new StringContent(
				$"client_id={_clientId}&grant_type=client_credentials&client_secret={_clientSecret}",
				Encoding.UTF8,
				"application/x-www-form-urlencoded");
			var response = await httpClient
				.PostAsync("as/token.oauth2", stringContent, cancellationToken)
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
			_accessToken = accessTokenResponse.AccessToken;
			_authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", _accessToken);
		}

		protected override async Task<HttpResponseMessage> SendAsync(
			HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			if (_accessToken is null)
			{
				await AuthenticateAsync(cancellationToken)
					.ConfigureAwait(false);
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
