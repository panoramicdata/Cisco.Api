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
		private readonly HttpClient _restHttpClient;
		private readonly HttpClient _soapHttpClient;
		private readonly bool _shouldDisposeHttpClient;
		private bool disposedValue;

		public CiscoClient(
			CiscoClientOptions options,
			ILogger? logger = default)
		{
			_logger = logger ?? NullLogger.Instance;

			if (options is null)
			{
				throw new ArgumentNullException(nameof(options));
			}
			if (options.ClientId is null)
			{
				throw new ArgumentException("Options ClientId must be set", nameof(options));
			}
			if (options.ClientSecret is null)
			{
				throw new ArgumentException("Options ClientSecret must be set", nameof(options));
			}

			_restHttpClient = new HttpClient(
				new AuthenticatedHttpClientHandler(
					"https://cloudsso.cisco.com/",
					"as/token.oauth2",
					options.ClientId,
					options.ClientSecret,
					options.Token,
					_logger)
			)
			{
				BaseAddress = new Uri("https://api.cisco.com/")
			};

			_soapHttpClient = new HttpClient(
				new AuthenticatedHttpClientHandler(
					"https://api.cisco.com/",
					"pss/token",
					options.ClientId,
					options.ClientSecret,
					options.Token,
					_logger)
			)
			{
				BaseAddress = new Uri("https://api.cisco.com/pss/v1.0/")
			};

			var refitSettings = new RefitSettings
			{
				UrlParameterFormatter = new CustomUrlParameterFormatter()
			};

			// Interfaces
			Eox = RestService.For<IEox>(_restHttpClient, refitSettings);
			Hello = RestService.For<IHello>(_restHttpClient);
			ProductInfo = RestService.For<IProductInfo>(_restHttpClient, refitSettings);
			Pss = new PssServices(_soapHttpClient);
			SerialNumberToInfo = RestService.For<ISerialNumberToInfo>(_restHttpClient, refitSettings);
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
					_restHttpClient?.Dispose();
					_soapHttpClient?.Dispose();
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
