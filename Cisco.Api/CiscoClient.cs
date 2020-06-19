using Cisco.Api.Interfaces;
using Cisco.Api.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Refit;
using System;
using System.Net.Http;

namespace Cisco.Api
{
	public partial class CiscoClient : IDisposable
	{
		private readonly ILogger _logger;
		private readonly HttpClient _httpClient;
		private readonly bool _shouldDisposeHttpClient;
		private bool disposedValue;

		public CiscoClient(
			CiscoClientOptions options,
			ILogger? logger = default)
		{
			if (options is null)
			{
				throw new ArgumentNullException(nameof(options));
			}
			_logger = logger ?? NullLogger.Instance;

			if (options.HttpClient != null)
			{
				// An HttpClient was provided to us.
				_httpClient = options.HttpClient;
			}
			else
			{
				// We are creating an HttpClient (one was not provided), so set _httpClient so that we know to dispose of it later.
				_httpClient = new HttpClient(
					new AuthenticatedHttpClientHandler(
						options.ClientId ?? throw new ArgumentException("Options ClientId must be set when no HttpClient is provided", nameof(options)),
						options.ClientSecret ?? throw new ArgumentException("Options ClientSecret must be set when no HttpClient is provided", nameof(options)),
						options.Token,
						_logger)
				)
				{
					BaseAddress = options.Uri ?? throw new ArgumentException("Options ClientId must not be null when no HttpClient is provided", nameof(options))
				};
				_shouldDisposeHttpClient = true;
			}

			var refitSettings = new RefitSettings
			{
				UrlParameterFormatter = new CustomUrlParameterFormatter()
			};

			// Interfaces
			Eox = RestService.For<IEox>(_httpClient, refitSettings);
			Hello = RestService.For<IHello>(_httpClient);
			ProductInfo = RestService.For<IProductInfo>(_httpClient, refitSettings);
			Pss = RestService.For<IPss>(_httpClient);
			SerialNumberToInfo = RestService.For<ISerialNumberToInfo>(_httpClient, refitSettings);
		}

		public IHello Hello { get; set; }

		public IEox Eox { get; set; }

		public IProductInfo ProductInfo { get; set; }

		public IPss Pss { get; set; }

		public ISerialNumberToInfo SerialNumberToInfo { get; set; }

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					if (_shouldDisposeHttpClient)
					{
						_httpClient?.Dispose();
					}
				}

				disposedValue = true;
			}
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
