using Cisco.Api.Implementations;
using Cisco.Api.Interfaces;
using Cisco.Api.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Refit;
using System;
using System.Linq;
using System.Net.Http;

namespace Cisco.Api;

public partial class CiscoClient : IDisposable
{
	private readonly ILogger _logger;
	private readonly HttpClient _restHttpClient;
	private readonly HttpClient _restEnterpriseAgreementClient;
	private readonly HttpClient _restUmbrellaClient;
	private readonly HttpClient _restPssClient;
	private readonly HttpClient _restPXCloudClient;
	private readonly HttpClient _restSmartAccountsAndLicensingClient;
	private readonly HttpClient _soapHttpClient;
	private bool disposedValue;

	public IEnterpriseAgreement EnterpriseAgreement { get; set; }

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

	public ISmartAccountsAndLicensing SmartAccountsAndLicensing { get; set; }

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

		/////////////////////////////
		/// Some of the following APIs expect "application/json" as the content type
		/// Enterprise Agreement and Smart Accounts And Licensing, even for GET requests with no body.
		var alternativeOptionsWithContentTypeAsJson = new CiscoClientOptions
		{
			ClientId = options.ClientId,
			ClientSecret = options.ClientSecret,
			HttpClientTimeoutSeconds = options.HttpClientTimeoutSeconds,
			// IMPORTANT: EA API requires application/json content type
			// for all requests, regardless of whether there is a body or not
			UseJsonContentType = true
		};

		_restHttpClient = CreateAuthenticatedHttpClient("https://id.cisco.com/oauth2/default/v1/token", "https://apix.cisco.com/", options);
		_restEnterpriseAgreementClient = CreateAuthenticatedHttpClient("https://cloudsso.cisco.com/as/token.oauth2", "https://swapi.cisco.com/services/api/enterprise-agreements", alternativeOptionsWithContentTypeAsJson);
		_restUmbrellaClient = CreateUmbrellaHttpClient(options);
		_restPssClient = CreateAuthenticatedHttpClient("https://api.cisco.com/pss/token", "https://api.cisco.com/", options);
		_restPXCloudClient = CreatePxCloudHttpClient(options);
		_soapHttpClient = CreateAuthenticatedHttpClient("https://api.cisco.com/pss/token", "https://api.cisco.com/pss/v1.0/", options);
		_restSmartAccountsAndLicensingClient = CreateAuthenticatedHttpClient("https://cloudsso.cisco.com/as/token.oauth2", "https://swapi.cisco.com/services/api/smart-accounts-and-licensing", alternativeOptionsWithContentTypeAsJson);

		var refitSettings = new RefitSettings
		{
			UrlParameterFormatter = new CustomUrlParameterFormatter(),
			ContentSerializer = new NewtonsoftJsonContentSerializer(
				new JsonSerializerSettings
				{
					NullValueHandling = NullValueHandling.Ignore,
					Converters = [new StringEnumConverter()]
				}
			)
		};

		// Interfaces
		EnterpriseAgreement = RestService.For<IEnterpriseAgreement>(_restEnterpriseAgreementClient, refitSettings);
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
		SmartAccountsAndLicensing = RestService.For<ISmartAccountsAndLicensing>(_restSmartAccountsAndLicensingClient, refitSettings);
		SoftwareSuggestion = RestService.For<ISoftwareSuggestion>(_restHttpClient, refitSettings);
		Umbrella = RestService.For<IUmbrella>(_restUmbrellaClient, refitSettings);
	}

	private HttpClient CreateAuthenticatedHttpClient(string tokenUri, string baseAddress, CiscoClientOptions options) => new(
		new AuthenticatedHttpClientHandler(new(tokenUri), options, _logger))
	{
		BaseAddress = new(baseAddress),
		Timeout = TimeSpan.FromSeconds(options.HttpClientTimeoutSeconds)
	};

	private HttpClient CreateUmbrellaHttpClient(CiscoClientOptions options)
	{
		HttpClientHandler handler = options.ClientCredentialsNotSupported is not null
			? new AuthenticatedFastUmbrellaHttpClientHandler(new("https://api.umbrella.com/auth/v2/token"), options, _logger)
			: new AuthenticatedUmbrellaHttpClientHandler(new("https://api.umbrella.com/auth/v2/token"), options, _logger);

		return new HttpClient(handler)
		{
			BaseAddress = new("https://api.umbrella.com/"),
			Timeout = TimeSpan.FromSeconds(options.HttpClientTimeoutSeconds)
		};
	}

	private HttpClient CreatePxCloudHttpClient(CiscoClientOptions options) => new(
		new AuthenticatedHttpClientHandler(
			new("https://id.cisco.com/oauth2/aus1o4emxorc3wkEe5d7/v1/token"),
			options, _logger, "api.authz.iam.manage"))
	{
		BaseAddress = new("https://api-cx.cisco.com/"),
		Timeout = TimeSpan.FromSeconds(options.HttpClientTimeoutSeconds)
	};

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
