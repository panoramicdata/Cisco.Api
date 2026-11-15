using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cisco.Api.Security;

internal abstract partial class CustomHttpClientHandler
{
	private async Task LogResponseHeadersIfNeeded(HttpResponseMessage response)
	{
		if (_logger.IsEnabled(LevelToLogAt))
		{
			await LogResponseHeaders(response).ConfigureAwait(false);
		}
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
			request.ToRedactedString()
		);
		await LogContentBlockAsync(request.Content, logAsError, "RequestContent\r\n{RequestContext}").ConfigureAwait(false);
	}

	private async Task LogResponseHeaders(HttpResponseMessage httpResponseMessage, bool logAsError = false)
	{
		// Use logging override if set
		_logger.Log(
			logAsError ? LogLevel.Error : LevelToLogAt,
			"Response\r\n{Response}",
			httpResponseMessage.ToRedactedString()
		);

		await LogContentBlockAsync(httpResponseMessage.Content, logAsError, "ResponseContent\r\n{ResponseContent}").ConfigureAwait(false);
	}

	private async Task LogContentBlockAsync(HttpContent? content, bool logAsError, string template)
	{
		if (content == null)
		{
			return;
		}

		var body = await content
			.ReadAsStringAsync()
			.ConfigureAwait(false);

		_logger.Log(
			logAsError ? LogLevel.Error : LevelToLogAt,
			template,
			body);
	}
}
