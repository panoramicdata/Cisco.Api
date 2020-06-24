using Cisco.Api.Data.Pss;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Interfaces
{
	/// <summary>
	/// PSS calls
	/// </summary>
	public interface IPss
	{
		Task<ContractCoverageResponse> GetContractCoverageAsync(
			ContractCoverageRequest request,
			CancellationToken cancellationToken);

		Task<CustomerInventoryDetailPaginatedResponse> GetCustomerInventoryPaginatedDetailsAsync(
			CustomerInventoryDetailPaginatedRequest request,
			CancellationToken cancellationToken);

		Task<CustomersInventoryResponse> GetCustomerInventoryAsync(
			CustomersInventoryRequest customerInventoryRequest,
			CancellationToken cancellationToken);

		Task<EoswmLifecycleResponse> GetEoswmLifecycleAsync(
			EoswmLifecycleRequest request,
			CancellationToken cancellationToken);

		Task<FieldNoticesResponse> GetFieldNoticesAsync(
			FieldNoticesRequest request,
			CancellationToken cancellationToken);

		Task<HardwareEoxResponse> GetHardwareEoxAsync(
			HardwareEoxRequest request,
			CancellationToken cancellationToken);

		Task<HardwareEoxBulletinResponse> GetHardwareEoxBulletinAsync(
			HardwareEoxBulletinRequest request,
			CancellationToken cancellationToken);

		Task<PsirtResponse> GetPsirtAsync(
			PsirtRequest request,
			CancellationToken cancellationToken);
		Task<SoftwareEoxResponse> GetSoftwareEoxAsync(
			SoftwareEoxRequest request,
			CancellationToken cancellationToken);

		Task<SoftwareEoxBulletinResponse> GetSoftwareEoxBulletinAsync(
			SoftwareEoxBulletinRequest request,
			CancellationToken cancellationToken);
	}
}