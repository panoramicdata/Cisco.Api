using Cisco.Api.Data.Pss;
using Cisco.Api.Interfaces;
using SimpleSOAPClient;
using SimpleSOAPClient.Helpers;
using SimpleSOAPClient.Models;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api
{
	public class PssServices : IPss
	{
		private readonly SoapClient _soapClient;

		public PssServices(HttpClient soapHttpClient)
		{
			_soapClient = SoapClient.Prepare(soapHttpClient);
		}
		private async Task<TResponse> GetAsync<TRequest, TResponse>(
			string v1,
			string v2,
			TRequest request,
			CancellationToken cancellationToken
		)
		{
			var requestEnvelope = SoapEnvelope
				.Prepare()
				.Body(request);

			var responseEnvelope = await _soapClient.SendAsync(
				v1,
				v2,
				requestEnvelope,
				cancellationToken)
				.ConfigureAwait(false);

			return responseEnvelope
				.Body<TResponse>();
		}

		public Task<CustomersInventoryResponse> GetCustomerInventoryAsync(
			CustomersInventoryRequest request,
			CancellationToken cancellationToken)
			=> GetAsync<CustomersInventoryRequest, CustomersInventoryResponse>(
				"InventoryService",
				"getCustomerInventoryIds",
				request,
				cancellationToken);

		public Task<CustomerInventoryDetailsResponse> GetCustomerInventoryDetailsAsync(
			CustomerInventoryDetailsRequest request,
			CancellationToken cancellationToken)
			=> GetAsync<CustomerInventoryDetailsRequest, CustomerInventoryDetailsResponse>(
				"InventoryService",
				"getCustomerInventoryDetails",
				request,
				cancellationToken);

		public Task<CustomerInventoryDetailPaginatedResponse> GetCustomerInventoryPaginatedDetailsAsync(
			CustomerInventoryDetailPaginatedRequest request,
			CancellationToken cancellationToken)
			=> GetAsync<CustomerInventoryDetailPaginatedRequest, CustomerInventoryDetailPaginatedResponse>(
				"InventoryService",
				"getCustomerInventoryPaginatedDetails",
				request,
				cancellationToken);

		public Task<ContractCoverageResponse> GetContractCoverageAsync(
			ContractCoverageRequest request,
			CancellationToken cancellationToken)
			=> GetAsync<ContractCoverageRequest, ContractCoverageResponse>(
				"ContractService",
				"getContractCoverageDetails",
				request,
				cancellationToken);

		public Task<SoftwareEoxResponse> GetSoftwareEoxAsync(
			SoftwareEoxRequest request,
			CancellationToken cancellationToken)
			=> GetAsync<SoftwareEoxRequest, SoftwareEoxResponse>(
				"SwEoxAlertService",
				"getSoftwareEox",
				request,
				cancellationToken);

		public Task<SoftwareEoxBulletinResponse> GetSoftwareEoxBulletinAsync(
			SoftwareEoxBulletinRequest request,
			CancellationToken cancellationToken)
			=> GetAsync<SoftwareEoxBulletinRequest, SoftwareEoxBulletinResponse>(
				"SwEoxAlertService",
				"getSoftwareEoxBulletin",
				request,
				cancellationToken);

		public Task<HardwareEoxResponse> GetHardwareEoxAsync(
			HardwareEoxRequest request,
			CancellationToken cancellationToken)
			=> GetAsync<HardwareEoxRequest, HardwareEoxResponse>(
				"HwEoxAlertService",
				"getHwEox",
				request,
				cancellationToken);

		public Task<HardwareEoxBulletinResponse> GetHardwareEoxBulletinAsync(
			HardwareEoxBulletinRequest request,
			CancellationToken cancellationToken)
			=> GetAsync<HardwareEoxBulletinRequest, HardwareEoxBulletinResponse>(
				"HwEoxAlertService",
				"getHwEoxBulletin",
				request,
				cancellationToken);

		public Task<EoswmLifecycleResponse> GetEoswmLifecycleAsync(
			EoswmLifecycleRequest request,
			CancellationToken cancellationToken)
			=> GetAsync<EoswmLifecycleRequest, EoswmLifecycleResponse>(
				"InventoryService",
				"getCustomerInventoryIds",
				request,
				cancellationToken);

		public Task<PsirtResponse> GetPsirtAsync(
			PsirtRequest request,
			CancellationToken cancellationToken)
			=> GetAsync<PsirtRequest, PsirtResponse>(
				"PSIRTAlertService",
				"getPSIRT",
				request,
				cancellationToken);

		public Task<FieldNoticesResponse> GetFieldNoticesAsync(
			FieldNoticesRequest request,
			CancellationToken cancellationToken)
			=> GetAsync<FieldNoticesRequest, FieldNoticesResponse>(
				"FNAlertService",
				"getFN",
				request,
				cancellationToken);
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
}