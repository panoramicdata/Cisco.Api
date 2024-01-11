using Cisco.Api.Interfaces;
using Cisco.Api.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Cisco.Api;

public partial class CiscoClient : IDisposable
{
	private readonly ILogger _logger;
	private readonly HttpClient _restHttpClient;
	private readonly HttpClient _restUmbrellaClient;
	private readonly HttpClient _soapHttpClient;
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
				new("https://id.cisco.com/oauth2/default/v1/token"),
				options,
				_logger)
		)
		{
			BaseAddress = new("https://apix.cisco.com/"),
			Timeout = TimeSpan.FromSeconds(options.HttpClientTimeoutSeconds)
		};

		// Umbrella uses a different endpoint
		// https://docs.umbrella.com/umbrella-api/docs/umbrella-api-quick-start
		_restUmbrellaClient = new HttpClient(
			new AuthenticatedHttpClientHandler(
				new("https://api.umbrella.com/auth/v2/token"),
				options,
				_logger))
		{
			BaseAddress = new("https://api.umbrella.com/"),
			Timeout = TimeSpan.FromSeconds(options.HttpClientTimeoutSeconds)
		};


		_soapHttpClient = new HttpClient(
			new AuthenticatedHttpClientHandler(
				new("https://api.cisco.com/pss/token"),
				options,
				_logger)
		)
		{
			BaseAddress = new("https://api.cisco.com/pss/v1.0/"),
			Timeout = TimeSpan.FromSeconds(options.HttpClientTimeoutSeconds)
		};

		var refitSettings = new RefitSettings
		{
			UrlParameterFormatter = new CustomUrlParameterFormatter(),
			ContentSerializer = new NewtonsoftJsonContentSerializer(
				new JsonSerializerSettings
				{
					// By default nulls should not be rendered out, this will allow the receiving API to apply any defaults.
					// Use [JsonProperty(NullValueHandling = NullValueHandling.Include)] to send
					// nulls for specific properties, i.e. disassociating port schedule ids from a port
					NullValueHandling = NullValueHandling.Ignore,
					//#if DEBUG
					//						MissingMemberHandling = MissingMemberHandling.Error,
					//#endif
					Converters = new List<JsonConverter> { new StringEnumConverter() }
				}
			)
		};

		// Interfaces
		Eox = RestService.For<IEox>(_restHttpClient, refitSettings);
		Hello = RestService.For<IHello>(_restHttpClient);
		ProductInfo = RestService.For<IProductInfo>(_restHttpClient, refitSettings);
		Psirt = RestService.For<IPsirt>(_restHttpClient, refitSettings);
		Pss = new PssServices(_soapHttpClient);
		SerialNumberToInfo = RestService.For<ISerialNumberToInfo>(_restHttpClient, refitSettings);
		SoftwareSuggestion = RestService.For<ISoftwareSuggestion>(_restHttpClient, refitSettings);
		Umbrella = RestService.For<IUmbrella>(_restUmbrellaClient, refitSettings);
	}

	public IEox Eox { get; set; }

	public IHello Hello { get; set; }

	public IPsirt Psirt { get; set; }

	public IProductInfo ProductInfo { get; set; }

	public IPss Pss { get; set; }

	public ISerialNumberToInfo SerialNumberToInfo { get; set; }

	public ISoftwareSuggestion SoftwareSuggestion { get; set; }

	public IUmbrella Umbrella { get; set; }

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
