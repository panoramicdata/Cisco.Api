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
		CancellationToken cancellationToken = default);

	/// <summary>
	/// List the internal networks
	/// </summary>
	/// <param name="name">The internal network label</param>
	/// <param name="page">Specifies a page number in the collection.</param>
	/// <param name="limit">Specifies the number of records to return per page.</param>
	/// <param name="cancellationToken"></param>
	[Get("/deployments/v2/internalnetworks")]
	Task<List<InternalNetwork>> ListInternalNetworksAsync(
		string? name = null,
		int? page = 1,
		int? limit = 100,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Get an internal network
	/// </summary>
	/// <param name="internalNetworkId">The origin ID (originId) of the internal network</param>
	/// <param name="cancellationToken"></param>
	[Get("/deployments/v2/internalnetworks/{internalNetworkId}")]
	Task<InternalNetwork> GetInternalNetworkAsync(
		int internalNetworkId,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Update an internal network
	/// </summary>
	/// <param name="internalNetworkId">The origin ID (originId) of the internal network</param>
	/// <param name="cancellationToken"></param>
	[Put("/deployments/v2/internalnetworks/{internalNetworkId}")]
	Task<InternalNetwork> UpdateInternalNetworkAsync(
		int internalNetworkId,
		[Body] InternalNetworksCreateUpdateRequest request,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Delete an internal network
	/// </summary>
	/// <param name="internalNetworkId">The origin ID (originId) of the internal network</param>
	/// <param name="cancellationToken"></param>
	[Delete("/deployments/v2/internalnetworks/{internalNetworkId}")]
	Task DeleteInternalNetworkAsync(
		int internalNetworkId,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// List Policies for an internal network
	/// </summary>
	/// <param name="internalNetworkId">The origin ID (originId) of the internal network</param>
	/// <param name="type">Specifies the type of Umbrella policy (either DNS or Web)</param>
	/// <param name="cancellationToken"></param>
	[Get("/deployments/v2/internalnetworks/{internalNetworkId}/policies")]
	Task<List<InternalNetworkPolicy>> ListPoliciesForInternalNetworkAsync(
		int internalNetworkId,
		string? type = "dns",
		CancellationToken cancellationToken = default);


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
		string? type = "dns",
		int? page = 1,
		int? limit = 100,
		CancellationToken cancellationToken = default);

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
		CancellationToken cancellationToken = default);

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
		CancellationToken cancellationToken = default);


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
		int? page = 1,
		int? limit = 100,
		CancellationToken cancellationToken = default);
}
