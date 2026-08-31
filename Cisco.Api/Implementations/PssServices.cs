using Cisco.Api.Data.Pss;
using Cisco.Api.Interfaces;
using SimpleSOAPClient;
using SimpleSOAPClient.Helpers;
using SimpleSOAPClient.Models;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Implementations;

/// <summary>
/// Represents the PSS services.
/// </summary>
/// <param name="soapHttpClient">The HTTP client used to send SOAP requests.</param>
public class PssServices(HttpClient soapHttpClient) : IPss
{
	private readonly SoapClient _soapClient = SoapClient.Prepare(soapHttpClient);

	private async Task<TResponse> GetAsync<TRequest, TResponse>(
			string url,
			string action,
			TRequest request,
			CancellationToken cancellationToken
		)
	{
		var requestEnvelope = SoapEnvelope
			.Prepare()
			.Body(request);

		var responseEnvelope = await _soapClient.SendAsync(
			url,
			action,
			requestEnvelope,
			cancellationToken)
			.ConfigureAwait(false);

		return responseEnvelope
			.Body<TResponse>();
	}

	/// <summary>
	/// Performs the get customers inventory ids operation.
	/// </summary>
	public Task<CustomersInventoryResponse> GetCustomersInventoryIdsAsync(
		CustomersInventoryRequest request,
		CancellationToken cancellationToken)
		=> GetAsync<CustomersInventoryRequest, CustomersInventoryResponse>(
			"InventoryService",
			"getCustomersInventoryIds",
			request,
			cancellationToken);

	/// <summary>
	/// Performs the get customer inventory details operation.
	/// </summary>
	public Task<CustomerInventoryDetailsResponse> GetCustomerInventoryDetailsAsync(
		CustomerInventoryDetailsRequest request,
		CancellationToken cancellationToken)
		=> GetAsync<CustomerInventoryDetailsRequest, CustomerInventoryDetailsResponse>(
			"InventoryService",
			"getCustomerInventoryDetails",
			request,
			cancellationToken);

	/// <summary>
	/// Performs the get customer extended inventory details operation.
	/// </summary>
	public Task<CustomerExtendedInventoryDetailsResponse> GetCustomerExtendedInventoryDetailsAsync(
		CustomerExtendedInventoryDetailsRequest request,
		CancellationToken cancellationToken)
		=> GetAsync<CustomerExtendedInventoryDetailsRequest, CustomerExtendedInventoryDetailsResponse>(
			"InventoryService",
			"getCustomerExtendedInventoryDetails",
			request,
			cancellationToken);

	/// <summary>
	/// Performs the get customer inventory paginated details operation.
	/// </summary>
	public Task<CustomerInventoryDetailPaginatedResponse> GetCustomerInventoryPaginatedDetailsAsync(
		CustomerInventoryDetailPaginatedRequest request,
		CancellationToken cancellationToken)
		=> GetAsync<CustomerInventoryDetailPaginatedRequest, CustomerInventoryDetailPaginatedResponse>(
			"InventoryService",
			"getCustomerInventoryPaginatedDetails",
			request,
			cancellationToken);

	/// <summary>
	/// Performs the get contract coverage operation.
	/// </summary>
	public Task<ContractCoverageResponse> GetContractCoverageAsync(
		ContractCoverageRequest request,
		CancellationToken cancellationToken)
		=> GetAsync<ContractCoverageRequest, ContractCoverageResponse>(
			"ContractService",
			"getContractCoverageDetails",
			request,
			cancellationToken);

	/// <summary>
	/// Performs the get software EoX operation.
	/// </summary>
	public Task<SoftwareEoxResponse> GetSoftwareEoxAsync(
		SoftwareEoxRequest request,
		CancellationToken cancellationToken)
		=> GetAsync<SoftwareEoxRequest, SoftwareEoxResponse>(
			"SwEoxAlertService",
			"getSoftwareEox",
			request,
			cancellationToken);

	/// <summary>
	/// Performs the get software EoX bulletin operation.
	/// </summary>
	public Task<SoftwareEoxBulletinResponse> GetSoftwareEoxBulletinAsync(
		SoftwareEoxBulletinRequest request,
		CancellationToken cancellationToken)
		=> GetAsync<SoftwareEoxBulletinRequest, SoftwareEoxBulletinResponse>(
			"SwEoxAlertService",
			"getSoftwareEoxBulletin",
			request,
			cancellationToken);

	/// <summary>
	/// Performs the get hardware EoX operation.
	/// </summary>
	public Task<HardwareEoxResponse> GetHardwareEoxAsync(
		HardwareEoxRequest request,
		CancellationToken cancellationToken)
		=> GetAsync<HardwareEoxRequest, HardwareEoxResponse>(
			"HwEoxAlertService",
			"getHwEox",
			request,
			cancellationToken);

	/// <summary>
	/// Performs the get hardware EoX bulletin operation.
	/// </summary>
	public Task<HardwareEoxBulletinResponse> GetHardwareEoxBulletinAsync(
		HardwareEoxBulletinRequest request,
		CancellationToken cancellationToken)
		=> GetAsync<HardwareEoxBulletinRequest, HardwareEoxBulletinResponse>(
			"HwEoxAlertService",
			"getHwEoxBulletin",
			request,
			cancellationToken);

	/// <summary>
	/// Performs the get EOSWM lifecycle operation.
	/// </summary>
	public Task<EoswmLifecycleResponse> GetEoswmLifecycleAsync(
		EoswmLifecycleRequest request,
		CancellationToken cancellationToken)
		=> GetAsync<EoswmLifecycleRequest, EoswmLifecycleResponse>(
			"InventoryService",
			"getCustomerInventoryIds",
			request,
			cancellationToken);

	/// <summary>
	/// Performs the get PSIRT operation.
	/// </summary>
	public Task<PsirtResponse> GetPsirtAsync(
		PsirtRequest request,
		CancellationToken cancellationToken)
		=> GetAsync<PsirtRequest, PsirtResponse>(
			"PSIRTAlertService",
			"getPSIRT",
			request,
			cancellationToken);

	/// <summary>
	/// Performs the get PSIRT details operation.
	/// </summary>
	public Task<PsirtDetailsResponse> GetPsirtDetailsAsync(
		PsirtDetailsRequest request,
		CancellationToken cancellationToken)
		=> GetAsync<PsirtDetailsRequest, PsirtDetailsResponse>(
			"PSIRTAlertService",
			"getPSIRTDetails",
			request,
			cancellationToken
			);

	/// <summary>
	/// Performs the get field notices operation.
	/// </summary>
	public Task<FieldNoticesResponse> GetFieldNoticesAsync(
		FieldNoticesRequest request,
		CancellationToken cancellationToken)
		=> GetAsync<FieldNoticesRequest, FieldNoticesResponse>(
			"FNAlertService",
			"getFN",
			request,
			cancellationToken);
	/// <summary>
	/// Performs the get field notices details operation.
	/// </summary>
	public Task<FieldNoticesDetailsResponse> GetFieldNoticesDetailsAsync(
		FieldNoticesDetailsRequest request,
		CancellationToken cancellationToken)
		=> GetAsync<FieldNoticesDetailsRequest, FieldNoticesDetailsResponse>(
			"FNAlertService",
			"getFNDetails",
			request,
			cancellationToken
			);
}