using Cisco.Api.Implementations;
using Cisco.Api.Data.Pss;
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
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api;

/// <summary>
/// Represents the Cisco client.
/// </summary>
public class CiscoClient : IDisposable
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

	/// <summary>
	/// Gets or sets the enterprise agreement.
	/// </summary>
	public IEnterpriseAgreement EnterpriseAgreement { get; set; }

	/// <summary>
	/// Gets or sets the EoX.
	/// </summary>
	public IEox Eox { get; set; }

	/// <summary>
	/// Gets or sets the hello.
	/// </summary>
	public IHello Hello { get; set; }

	/// <summary>
	/// Gets or sets the PSIRT.
	/// </summary>
	public IPsirt Psirt { get; set; }

	/// <summary>
	/// Gets or sets the product info.
	/// </summary>
	public IProductInfo ProductInfo { get; set; }

	/// <summary>
	/// Gets or sets the PSS.
	/// </summary>
	public IPss Pss { get; set; }

	/// <summary>
	/// Gets or sets the PSS configs.
	/// </summary>
	public IPssConfigs PssConfigs { get; set; }

	/// <summary>
	/// Gets or sets the PX cloud.
	/// </summary>
	public IPxCloud PxCloud { get; set; }

	/// <summary>
	/// Gets or sets the PX cloud reports.
	/// </summary>
	public IPxCloudReports PxCloudReports { get; set; }

	/// <summary>
	/// Gets or sets the security advisory.
	/// </summary>
	public ISecurityAdvisory SecurityAdvisory { get; set; }

	/// <summary>
	/// Gets or sets the serial number to info.
	/// </summary>
	public ISerialNumberToInfo SerialNumberToInfo { get; set; }

	/// <summary>
	/// Gets or sets the smart accounts and licensing.
	/// </summary>
	public ISmartAccountsAndLicensing SmartAccountsAndLicensing { get; set; }

	/// <summary>
	/// Gets or sets the software suggestion.
	/// </summary>
	public ISoftwareSuggestion SoftwareSuggestion { get; set; }

	/// <summary>
	/// Gets or sets the umbrella.
	/// </summary>
	public IUmbrella Umbrella { get; set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="CiscoClient"/> class.
	/// </summary>
	/// <param name="options">The client configuration.</param>
	public CiscoClient(CiscoClientOptions options)
		: this(options, null)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="CiscoClient"/> class.
	/// </summary>
	/// <param name="options">The client configuration.</param>
	/// <param name="logger">The optional logger.</param>
	public CiscoClient(
		CiscoClientOptions options,
		ILogger? logger)
	{
		_logger = logger ?? NullLogger.Instance;

		ArgumentNullException.ThrowIfNull(options);
		ValidateOptions(options);

		/////////////////////////////
		// Some of the following APIs expect "application/json" as the content type:
		// Enterprise Agreement and Smart Accounts And Licensing, even for GET requests with no body.
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
		_restEnterpriseAgreementClient = CreateAuthenticatedHttpClient("https://id.cisco.com/oauth2/default/v1/token", "https://swapi.cisco.com/services/api/enterprise-agreements", alternativeOptionsWithContentTypeAsJson);
		_restUmbrellaClient = CreateUmbrellaHttpClient(options);
		_restPssClient = CreateAuthenticatedHttpClient("https://api.cisco.com/pss/token", "https://api.cisco.com/", options);
		_restPXCloudClient = CreatePxCloudHttpClient(options);
		_soapHttpClient = CreateAuthenticatedHttpClient("https://api.cisco.com/pss/token", "https://api.cisco.com/pss/v1.0/", options);
		_restSmartAccountsAndLicensingClient = CreateAuthenticatedHttpClient("https://id.cisco.com/oauth2/default/v1/token", "https://swapi.cisco.com/services/api/smart-accounts-and-licensing", alternativeOptionsWithContentTypeAsJson);

		var refitSettings = CreateRefitSettings();

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

	private static void ValidateOptions(CiscoClientOptions options)
	{
		if (options.ClientCredentialsNotSupported is not null)
		{
			// This property is only for unofficial use with Umbrella to improve performance by avoiding the rate limiter.
			if (!options.ClientCredentialsNotSupported.Any())
			{
				throw new ArgumentException("There must be at least one set of credentials.", nameof(options));
			}

			return;
		}

		if (options.ClientId is null)
		{
			throw new ArgumentException("Options ClientId must be set", nameof(options));
		}

		if (options.ClientSecret is null)
		{
			throw new ArgumentException("Options ClientSecret must be set", nameof(options));
		}
	}

	private static RefitSettings CreateRefitSettings() => new()
	{
		UrlParameterFormatter = new CustomUrlParameterFormatter(),
		ContentSerializer = new NewtonsoftJsonContentSerializer(
			new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore,
				Converters = [new StringEnumConverter()]
			})
	};

	/// <summary>
	/// Checks which Cisco APIs can be accessed with the configured credentials.
	/// </summary>
	/// <param name="cancellationToken">A token used to cancel the checks.</param>
	/// <returns>The API access results.</returns>
	public async Task<ApiAccess> GetApiAccessAsync(CancellationToken cancellationToken)
	{
		_logger.LogDebug("Checking API access...");
		var apiAccess = new ApiAccess();
		var todayMidnight = DateTime.Today;
		var yesterdayMidnight = todayMidnight.AddDays(-1);

		// MS-19906: these calls use harmless sample values where each API requires input.
		var enterpriseAgreementTask = EnterpriseAgreement.GetConsumptionReportForAllSubscriptionsAssociatedWithSmartAccountDomainAsync("demo.mule.cisco.com", cancellationToken);
		var eoxTask = Eox.GetByDatesAsync(yesterdayMidnight, todayMidnight, 1, cancellationToken);
		var helloTask = Hello.HelloAsync(cancellationToken);
		var psirtTask = Psirt.GetLatestAsync(1, cancellationToken);
		var productInfoTask = ProductInfo.GetBySerialNumbersAsync(["123"], cancellationToken);
		var pssTask = Pss.GetFieldNoticesAsync(new FieldNoticesRequest(), cancellationToken);
		var serialNumberToInfoTask = SerialNumberToInfo.GetCoverageStatusBySerialNumbersAsync(["123"], cancellationToken);
		var softwareSuggestionTask = SoftwareSuggestion.GetByProductIdsAsync(["C9200"], 1, cancellationToken);
		var smartAccountsAndLicensingTask = SmartAccountsAndLicensing.SearchSmartAccountsAsync("123", null, 50, 0, null, cancellationToken);
		var umbrellaTask = Umbrella.ListSitesAsync(1, 100, cancellationToken);

		apiAccess.EnterpriseAgreement = await TryApiAsync("Enterprise Agreement", enterpriseAgreementTask).ConfigureAwait(false);
		apiAccess.Eox = await TryApiAsync("Eox", eoxTask).ConfigureAwait(false);
		apiAccess.Hello = await TryApiAsync("Hello", helloTask).ConfigureAwait(false);
		apiAccess.Psirt = await TryApiAsync("Psirt", psirtTask).ConfigureAwait(false);
		apiAccess.ProductInfo = await TryApiAsync("ProductInfo", productInfoTask).ConfigureAwait(false);
		apiAccess.Pss = await TryApiAsync("Pss", pssTask).ConfigureAwait(false);
		apiAccess.SerialNumberToInfo = await TryApiAsync("SerialNumberToInfo", serialNumberToInfoTask).ConfigureAwait(false);
		apiAccess.SoftwareSuggestion = await TryApiAsync("SoftwareSuggestion", softwareSuggestionTask).ConfigureAwait(false);
		apiAccess.SmartAccountsAndLicensing = await TryApiAsync("SmartAccountsAndLicensing", smartAccountsAndLicensingTask).ConfigureAwait(false);
		apiAccess.Umbrella = await TryApiAsync("Umbrella", umbrellaTask).ConfigureAwait(false);

		return apiAccess;
	}

	private async Task<bool> TryApiAsync(string name, Task task)
	{
		try
		{
			_logger.LogDebug("Checking {Name}", name);
			await task.ConfigureAwait(false);
			_logger.LogDebug("{Name} succeeded", name);
			return true;
		}
		catch (Exception ex)
		{
			_logger.LogDebug(ex, "{Name} failed", name);
			return false;
		}
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

	/// <summary>
	/// Releases the resources used by this instance.
	/// </summary>
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

	/// <summary>
	/// Releases the resources used by this instance.
	/// </summary>
	public void Dispose()
	{
		// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
