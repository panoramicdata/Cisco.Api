﻿using Cisco.Api.Implementations;
using Cisco.Api.Interfaces;
using Cisco.Api.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Cisco.Api;

public partial class CiscoClient : IDisposable
{
	private readonly ILogger _logger;
	private readonly HttpClient _restHttpClient;
	private readonly HttpClient _restUmbrellaClient;
	private readonly HttpClient _restPssClient;
	private readonly HttpClient _restPXCloudClient;
	private readonly HttpClient _soapHttpClient;
	private bool disposedValue;

	public IEox Eox { get; set; }

	public IHello Hello { get; set; }

	public IPsirt Psirt { get; set; }

	public IProductInfo ProductInfo { get; set; }

	public IPss Pss { get; set; }

	public IPssConfigs PssConfigs { get; set; }

	public IPxCloud PxCloud { get; set; }

	public IPxCloudReports PxCloudReports { get; set; }

	public ISecurityAdvisory SecurityAdvisory { get; set; }

	public ISerialNumberToInfo SerialNumberToInfo { get; set; }

	public ISoftwareSuggestion SoftwareSuggestion { get; set; }

	public IUmbrella Umbrella { get; set; }

	public CiscoClient(
		CiscoClientOptions options,
		ILogger? logger = default)
	{
		_logger = logger ?? NullLogger.Instance;

		if (options is null)
		{
			throw new ArgumentNullException(nameof(options));
		}

		if (options.ClientCredentialsNotSupported is not null)
		{
			// This property is only for un-official use with Umbrella to improve performance by avoiding the rate limiter.
			if (!options.ClientCredentialsNotSupported.Any())
			{
				throw new ArgumentException("There must be at least one set of credentials.", nameof(options));
			}
		}
		else
		{
			// Standard - single instance credentials for all other clients

			if (options.ClientId is null)
			{
				throw new ArgumentException("Options ClientId must be set", nameof(options));
			}

			if (options.ClientSecret is null)
			{
				throw new ArgumentException("Options ClientSecret must be set", nameof(options));
			}
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

		///////////////
		// Umbrella uses a different client handler to support API keys
		// https://docs.umbrella.com/umbrella-api/docs/umbrella-api-quick-start

		if (options.ClientCredentialsNotSupported is not null)
		{
			// Supports multiple credentials to circumvent rate limiting
			_restUmbrellaClient = new HttpClient(
				new AuthenticatedFastUmbrellaHttpClientHandler(
					new("https://api.umbrella.com/auth/v2/token"),
					options,
					_logger))
			{
				BaseAddress = new("https://api.umbrella.com/"),
				Timeout = TimeSpan.FromSeconds(options.HttpClientTimeoutSeconds)
			};
		}
		else
		{
			_restUmbrellaClient = new HttpClient(
				new AuthenticatedUmbrellaHttpClientHandler(
					new("https://api.umbrella.com/auth/v2/token"),
					options,
					_logger))
			{
				BaseAddress = new("https://api.umbrella.com/"),
				Timeout = TimeSpan.FromSeconds(options.HttpClientTimeoutSeconds)
			};
		}

		///////////////
		/// PSS Configs

		// Needs the old Cisco token format
		_restPssClient = new HttpClient(
			new AuthenticatedHttpClientHandler(
				new("https://api.cisco.com/pss/token"),
				options,
				_logger)
)
		{
			BaseAddress = new("https://api.cisco.com/"),
			Timeout = TimeSpan.FromSeconds(options.HttpClientTimeoutSeconds)
		};

		///////////////
		/// PX Cloud

		var scope = "api.authz.iam.manage";
		_restPXCloudClient = new HttpClient(
			new AuthenticatedHttpClientHandler(
				new("https://id.cisco.com/oauth2/aus1o4emxorc3wkEe5d7/v1/token"),
				options,
				_logger,
				scope)
		)
		{
			BaseAddress = new("https://api-cx.cisco.com/"),
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
		PssConfigs = new PssConfigs(_restPssClient);
		PxCloudReports = new PxCloudReports(_restPXCloudClient);
		PxCloud = RestService.For<IPxCloud>(_restPXCloudClient, refitSettings);
		SecurityAdvisory = RestService.For<ISecurityAdvisory>(_restHttpClient, refitSettings);
		SerialNumberToInfo = RestService.For<ISerialNumberToInfo>(_restHttpClient, refitSettings);
		SoftwareSuggestion = RestService.For<ISoftwareSuggestion>(_restHttpClient, refitSettings);
		Umbrella = RestService.For<IUmbrella>(_restUmbrellaClient, refitSettings);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing)
			{
				_restHttpClient?.Dispose();
				_soapHttpClient?.Dispose();
				_restUmbrellaClient?.Dispose();
				_restPXCloudClient?.Dispose();
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
