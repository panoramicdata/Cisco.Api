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
		/// <summary>
		/// This service call provides all the various contract information
		/// (contract details, coverage, and siteaddress info).
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<ContractCoverageResponse> GetContractCoverageAsync(
			ContractCoverageRequest request,
			CancellationToken cancellationToken);

		/// <summary>
		/// This API call fetches a list of Customer ID’s, and Inventory ID’s.
		/// This is the second step in a four step process of obtaining service call related information.
		/// </summary>
		/// <param name="customersInventoryRequest"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<CustomersInventoryResponse> GetCustomersInventoryIdsAsync(
			CustomersInventoryRequest customersInventoryRequest,
			CancellationToken cancellationToken);

		/// <summary>
		/// This API service call returns a collection of Inventory elements
		/// (device details, chassis details) for a given Inventory.
		/// This is the third step in a four step process of obtaining service call related information.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<CustomerInventoryDetailsResponse> GetCustomerInventoryDetailsAsync(
			CustomerInventoryDetailsRequest request,
			CancellationToken cancellationToken);

		/// <summary>
		/// This API service call returns a collection of Inventory elements
		/// (device details, chassis details) for a given Inventory.
		/// This is the third step in a four step process of obtaining service call related information.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<CustomerExtendedInventoryDetailsResponse> GetCustomerExtendedInventoryDetailsAsync(
			CustomerExtendedInventoryDetailsRequest request,
			CancellationToken cancellationToken);

		/// <summary>
		/// This operation will return (in addition to what is returned by getCustomerInventoryDetails)
		/// a new section at the beginning of the response called 'pages'. This section has four parts:
		/// pageSize, pageCurrent, pageTotal, pidCount
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<CustomerInventoryDetailPaginatedResponse> GetCustomerInventoryPaginatedDetailsAsync(
			CustomerInventoryDetailPaginatedRequest request,
			CancellationToken cancellationToken);

		//Task<EoswmLifecycleResponse> GetEoswmLifecycleAsync(
		//	EoswmLifecycleRequest request,
		//	CancellationToken cancellationToken);

		/// <summary>
		/// This API service call gets all Field Notices for the specified customer, inventory and devices.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<FieldNoticesResponse> GetFieldNoticesAsync(
			FieldNoticesRequest request,
			CancellationToken cancellationToken);

		/// <summary>
		/// This API call fetches details about specified Field Notices.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<FieldNoticesDetailsResponse> GetFieldNoticesDetailsAsync(
			FieldNoticesDetailsRequest request,
			CancellationToken cancellationToken);

		/// <summary>
		/// This API call returns the hardware end-of-life id and product id information.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<HardwareEoxResponse> GetHardwareEoxAsync(
			HardwareEoxRequest request,
			CancellationToken cancellationToken);

		/// <summary>
		/// This API call fetches descriptive details for the given HWEoxId(s).
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<HardwareEoxBulletinResponse> GetHardwareEoxBulletinAsync(
			HardwareEoxBulletinRequest request,
			CancellationToken cancellationToken);

		/// <summary>
		/// This API service call gets all PSIRT Alerts for the specified customer, inventory and devices.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<PsirtResponse> GetPsirtAsync(
			PsirtRequest request,
			CancellationToken cancellationToken);

		/// <summary>
		/// This API call fetches details about specified PSIRTs.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<PsirtDetailsResponse> GetPsirtDetailsAsync(
			PsirtDetailsRequest request,
			CancellationToken cancellationToken);

		/// <summary>
		/// This API call fetches software end-of-life id and product id information.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<SoftwareEoxResponse> GetSoftwareEoxAsync(
			SoftwareEoxRequest request,
			CancellationToken cancellationToken);

		/// <summary>
		/// This API call fetches various software end-of-life bulletins information.
		/// </summary>
		/// <param name="request"></param>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		Task<SoftwareEoxBulletinResponse> GetSoftwareEoxBulletinAsync(
			SoftwareEoxBulletinRequest request,
			CancellationToken cancellationToken);
	}
}