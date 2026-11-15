using Microsoft.Extensions.Logging;


namespace Cisco.Api.Security;

internal abstract partial class CustomFastUmbrellaHttpClientHandler
{
	private async Task LogResponseHeadersIfNeeded(int credentialIndex, HttpResponseMessage response)
	{
		if (_logger.IsEnabled(LevelToLogAt))
		{
			await LogResponseHeaders(credentialIndex, response).ConfigureAwait(false);
		}
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
			request.ToRedactedString()
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
			httpResponseMessage.ToRedactedString()
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
}
