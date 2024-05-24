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
		int? offset = 0,
		int? max = 10,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets list of partner contracts. Results are paginated, default 10 per page.
	/// </summary>
	/// <param name="offset">The number of items to skip before starting to collect the result set. Default value is 0.</param>
	/// <param name="max">The maximum number of items to return. Default value is 10, max 50.</param>
	/// <param name="cancellationToken"></param>
	[Get("/px/v1/contracts")]
	Task<Contracts> GetContractsAsync(
		int? offset = 0,
		int? max = 10,
		CancellationToken cancellationToken = default);

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
		int? offset = 0,
		int? max = 10,
		string? customerId = null,
		string? customerGUName = null,
		int? successTrackId = null,
		CancellationToken cancellationToken = default);

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
		int? offset = 0,
		int? max = 10,
		string? customerId = null,
		string? contractLineItemType = null,
		int? successTrackId = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets all the offers created by the Partners. Results are paginated.
	/// </summary>
	/// <param name="customerId">Unique identifier of the customer.</param>
	/// <param name="offset">The number of items to skip before starting to collect the result set. Default value is 0.</param>
	/// <param name="max">The maximum number of items to return. Max is 200.</param>
	/// <param name="successTrackId">Unique identifier of the Success Track.</param>
	/// <param name="offerStatus">Status of the Offer (Possible values : 'Unpublished', 'Published', 'Idle').</param>
	/// <param name="offerType">Type of the Offer (Possible Values : 'Accelerator', 'Ask the Experts').</param>
	/// <param name="cancellationToken"></param>
	//[Get("/px/v1/partnerOffers")]
	//Task<List<PartnerOffers>> GetPartnerOffersAsync(
	//	string customerId,
	//	int? offset = 0,
	//	int? max = null,
	//	int? successTrackId = null,
	//	string? offerStatus = null,
	//	string? offerType = null,
	//	CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets all the active and inactive sessions of all the Offers created by the Partners. Results are paginated.
	/// </summary>
	/// <param name="customerId">Unique identifier of the customer.</param>
	/// <param name="offset">The number of items to skip before starting to collect the result set. Default value is 0.</param>
	/// <param name="max">The maximum number of items to return. Max is 200.</param>
	/// <param name="successTrackId">Unique identifier of the Success Track.</param>
	/// <param name="offerType">Type of the Offer (Possible Values : 'Accelerator', 'Ask the Experts').</param>
	/// <param name="offerID">Unique Identifier of the offer.</param>
	/// <param name="cancellationToken"></param>
	//[Get("/px/v1/partnerOffers")]
	//Task<List<PartnerOffersSessions>> GetPartnerOffersSessionsAsync(
	//	string customerId,
	//	int? offset = 0,
	//	int? max = null,
	//	int? successTrackId = null,
	//	string? offerType = null,
	//	string? offerId = null,
	//	CancellationToken cancellationToken = default);
}