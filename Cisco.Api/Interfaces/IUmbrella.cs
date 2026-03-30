using Cisco.Api.Data.Umbrella;
using Refit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Interfaces;

public interface IUmbrella
{
	////////////////////////////////////////
	//// Internal Networks

	/// <summary>
	/// Create an internal network
	/// </summary>
	/// <param name="cancellationToken"></param>
	[Post("/deployments/v2/internalnetworks")]
	Task<InternalNetwork> CreateInternalNetworkAsync(
		[Body] InternalNetworksCreateUpdateRequest request,
		CancellationToken cancellationToken);

	Task<InternalNetwork> CreateInternalNetworkAsync([Body] InternalNetworksCreateUpdateRequest request)
		=> CreateInternalNetworkAsync(request, default);

	/// <summary>
	/// List the internal networks
	/// </summary>
	/// <param name="name">The internal network label</param>
	/// <param name="page">Specifies a page number in the collection.</param>
	/// <param name="limit">Specifies the number of records to return per page.</param>
	/// <param name="cancellationToken"></param>
	[Get("/deployments/v2/internalnetworks")]
	Task<List<InternalNetwork>> ListInternalNetworksAsync(
		string? name,
		int? page,
		int? limit,
		CancellationToken cancellationToken);

	Task<List<InternalNetwork>> ListInternalNetworksAsync(string? name, int? page, int? limit)
		=> ListInternalNetworksAsync(name, page, limit, default);

	Task<List<InternalNetwork>> ListInternalNetworksAsync()
		=> ListInternalNetworksAsync(null, 1, 100, default);

	/// <summary>
	/// Get an internal network
	/// </summary>
	/// <param name="internalNetworkId">The origin ID (originId) of the internal network</param>
	/// <param name="cancellationToken"></param>
	[Get("/deployments/v2/internalnetworks/{internalNetworkId}")]
	Task<InternalNetwork> GetInternalNetworkAsync(
		int internalNetworkId,
		CancellationToken cancellationToken);

	Task<InternalNetwork> GetInternalNetworkAsync(int internalNetworkId)
		=> GetInternalNetworkAsync(internalNetworkId, default);

	/// <summary>
	/// Update an internal network
	/// </summary>
	/// <param name="internalNetworkId">The origin ID (originId) of the internal network</param>
	/// <param name="cancellationToken"></param>
	[Put("/deployments/v2/internalnetworks/{internalNetworkId}")]
	Task<InternalNetwork> UpdateInternalNetworkAsync(
		int internalNetworkId,
		[Body] InternalNetworksCreateUpdateRequest request,
		CancellationToken cancellationToken);

	Task<InternalNetwork> UpdateInternalNetworkAsync(int internalNetworkId, [Body] InternalNetworksCreateUpdateRequest request)
		=> UpdateInternalNetworkAsync(internalNetworkId, request, default);

	/// <summary>
	/// Delete an internal network
	/// </summary>
	/// <param name="internalNetworkId">The origin ID (originId) of the internal network</param>
	/// <param name="cancellationToken"></param>
	[Delete("/deployments/v2/internalnetworks/{internalNetworkId}")]
	Task DeleteInternalNetworkAsync(
		int internalNetworkId,
		CancellationToken cancellationToken);

	Task DeleteInternalNetworkAsync(int internalNetworkId)
		=> DeleteInternalNetworkAsync(internalNetworkId, default);

	/// <summary>
	/// List Policies for an internal network
	/// </summary>
	/// <param name="internalNetworkId">The origin ID (originId) of the internal network</param>
	/// <param name="type">Specifies the type of Umbrella policy (either DNS or Web)</param>
	/// <param name="cancellationToken"></param>
	[Get("/deployments/v2/internalnetworks/{internalNetworkId}/policies")]
	Task<List<InternalNetworkPolicy>> ListPoliciesForInternalNetworkAsync(
		int internalNetworkId,
		string? type,
		CancellationToken cancellationToken);

	Task<List<InternalNetworkPolicy>> ListPoliciesForInternalNetworkAsync(int internalNetworkId, string? type)
		=> ListPoliciesForInternalNetworkAsync(internalNetworkId, type, default);

	Task<List<InternalNetworkPolicy>> ListPoliciesForInternalNetworkAsync(int internalNetworkId)
		=> ListPoliciesForInternalNetworkAsync(internalNetworkId, "dns", default);


	////////////////////////////////////////
	//// Policies

	/// <summary>
	/// List the Umbrella policies. You can filter by policy type. If you do not specify a policy type, Umbrella returns the DNS policies.
	/// </summary>
	/// <param name="type">Specifies the type of Umbrella policy (either DNS or Web)</param>
	/// <param name="page">Specifies a page number in the collection.</param>
	/// <param name="limit">Specifies the number of records to return per page.</param>
	/// <param name="cancellationToken"></param>
	[Get("/deployments/v2/policies")]
	Task<List<Policy>> ListPoliciesAsync(
		string? type,
		int? page,
		int? limit,
		CancellationToken cancellationToken);

	Task<List<Policy>> ListPoliciesAsync(string? type, int? page, int? limit)
		=> ListPoliciesAsync(type, page, limit, default);

	Task<List<Policy>> ListPoliciesAsync()
		=> ListPoliciesAsync("dns", 1, 100, default);

	/// <summary>
	/// Add an identity to a Umbrella DNS or Web policy. Policy changes may require up to twenty minutes to take affect globally. For DNS policies, TTLs, caching, and session reuse may cause some devices and domains to appear to take longer to update.
	/// </summary>
	/// <param name="originId">The origin ID of the identity</param>
	/// <param name="policyId">Specifies the Umbrella Policy (DNS or Web) ID</param>
	/// <param name="cancellationToken"></param>
	[Put("/deployments/v2/policies/{policyId}/identities/{originId}")]
	Task AddIdentityToPolicyAsync(
		int originId,
		int policyId,
		CancellationToken cancellationToken);

	Task AddIdentityToPolicyAsync(int originId, int policyId)
		=> AddIdentityToPolicyAsync(originId, policyId, default);

	/// <summary>
	/// Remove an identity from an Umbrella policy. Policy changes may require up to 20 minutes to take affect globally. Additionally, for DNS policies, TTLs, caching, and session reuse may cause some devices and domains to appear to take longer to update.
	/// </summary>
	/// <param name="originId">The origin ID of the identity</param>
	/// <param name="policyId">Specifies the Umbrella Policy (DNS or Web) ID</param>
	/// <param name="cancellationToken"></param>
	[Delete("/deployments/v2/policies/{policyId}/identities/{originId}")]
	Task DeleteIdentityFromPolicyAsync(
		int originId,
		int policyId,
		CancellationToken cancellationToken);

	Task DeleteIdentityFromPolicyAsync(int originId, int policyId)
		=> DeleteIdentityFromPolicyAsync(originId, policyId, default);


	////////////////////////////////////////
	/// Sites

	/// <summary>
	/// List the Sites for the organization
	/// </summary>
	/// <param name="page">Specifies a page number in the collection.</param>
	/// <param name="limit">Specifies the number of records to return per page.</param>
	/// <param name="cancellationToken"></param>
	[Get("/deployments/v2/sites")]
	Task<List<Site>> ListSitesAsync(
		int? page,
		int? limit,
		CancellationToken cancellationToken);

	Task<List<Site>> ListSitesAsync(int? page, int? limit)
		=> ListSitesAsync(page, limit, default);

	Task<List<Site>> ListSitesAsync()
		=> ListSitesAsync(1, 100, default);
}
