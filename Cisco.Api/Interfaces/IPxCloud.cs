using Cisco.Api.Data.PxCloud;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Interfaces;

public interface IPxCloud
{
	////////////////////////////////////////////////////////////////////////////
	/// Partner Business Insights

	/// <summary>
	/// Gets list of customers. Results are paginated, default 10 per page.
	/// </summary>
	/// <param name="offset">The number of items to skip before starting to collect the result set. Default value is 0.</param>
	/// <param name="max">The maximum number of items to return. Default value is 10, max 50.</param>
	/// <param name="cancellationToken"></param>
	[Get("/px/v1/customers")]
	Task<Customers> GetCustomersAsync(
		int? offset,
		int? max,
		CancellationToken cancellationToken);

	Task<Customers> GetCustomersAsync(int? offset, int? max)
		=> GetCustomersAsync(offset, max, default);

	Task<Customers> GetCustomersAsync()
		=> GetCustomersAsync(0, 10, default);

	/// <summary>
	/// Gets list of partner contracts. Results are paginated, default 10 per page.
	/// </summary>
	/// <param name="offset">The number of items to skip before starting to collect the result set. Default value is 0.</param>
	/// <param name="max">The maximum number of items to return. Default value is 10, max 50.</param>
	/// <param name="cancellationToken"></param>
	[Get("/px/v1/contracts")]
	Task<Contracts> GetContractsAsync(
		int? offset,
		int? max,
		CancellationToken cancellationToken);

	Task<Contracts> GetContractsAsync(int? offset, int? max)
		=> GetContractsAsync(offset, max, default);

	Task<Contracts> GetContractsAsync()
		=> GetContractsAsync(0, 10, default);

	/// <summary>
	/// Gets list of partner contracts with customers. Results are paginated, default 10 per page.
	/// </summary>
	/// <param name="offset">The number of items to skip before starting to collect the result set. Default value is 0.</param>
	/// <param name="max">The maximum number of items to return. Default value is 10, max 50.</param>
	/// <param name="customerId">Unique Identifier of the customer.</param>
	/// <param name="customerGUName">Customer global ultimate name. The customer identity name at the Enterprise level. Max length is 50 characters.</param>
	/// <param name="successTrackId">Unique identifier of the Success Track.</param>
	/// <param name="cancellationToken"></param>
	[Get("/px/v1/contractsWithCustomers")]
	Task<ContractsWithCustomers> GetContractsWithCustomersAsync(
		int? offset,
		int? max,
		string? customerId,
		string? customerGUName,
		int? successTrackId,
		CancellationToken cancellationToken);

	Task<ContractsWithCustomers> GetContractsWithCustomersAsync(int? offset, int? max, string? customerId, string? customerGUName, int? successTrackId)
		=> GetContractsWithCustomersAsync(offset, max, customerId, customerGUName, successTrackId, default);

	Task<ContractsWithCustomers> GetContractsWithCustomersAsync()
		=> GetContractsWithCustomersAsync(0, 10, null, null, null, default);

	/// <summary>
	/// Gets contract details. Results are paginated, default 10 per page.
	/// </summary>
	/// <param name="offset">The number of items to skip before starting to collect the result set. Default value is 0.</param>
	/// <param name="max">The maximum number of items to return. Default value is 10, max 50.</param>
	/// <param name="contractNumber">Contract ID of the service contract.</param>
	/// <param name="customerId">Unique identifier of the customer.</param>
	/// <param name="contractLineItemType">Contract Line Item type. Values can be PARENT,CHILD or STANDALONE. If contractLineItemType and componentType are provided in the request, contractLineItemType takes priority.</param>
	/// <param name="successTrackId">Unique identifier of the Success Track.</param>
	/// <param name="cancellationToken"></param>
	[Get("/px/v1/contract/details")]
	Task<ContractDetails> GetContractDetailsAsync(
		string contractNumber,
		int? offset,
		int? max,
		string? customerId,
		string? contractLineItemType,
		int? successTrackId,
		CancellationToken cancellationToken);

	Task<ContractDetails> GetContractDetailsAsync(string contractNumber, int? offset, int? max, string? customerId, string? contractLineItemType, int? successTrackId)
		=> GetContractDetailsAsync(contractNumber, offset, max, customerId, contractLineItemType, successTrackId, default);

	Task<ContractDetails> GetContractDetailsAsync(string contractNumber, int? offset, int? max)
		=> GetContractDetailsAsync(contractNumber, offset, max, null, null, null, default);

	Task<ContractDetails> GetContractDetailsAsync(string contractNumber)
		=> GetContractDetailsAsync(contractNumber, 0, 10, null, null, null, default);

	}