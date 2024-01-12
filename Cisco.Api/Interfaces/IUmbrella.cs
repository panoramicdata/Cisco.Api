using Cisco.Api.Data.Umbrella;
using Refit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Interfaces;

public interface IUmbrella
{
	/// <summary>
	/// List the internal networks
	/// </summary>
	/// <param name="cancellationToken"></param>
	[Get("/deployments/v2/internalnetworks")]
	Task<List<InternalNetwork>> ListInternalNetworksAsync(
		string? name,
		int? page = 1,
		int? limit = 100,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// List the Umbrella policies. You can filter by policy type. If you do not specify a policy type, Umbrella returns the DNS policies.
	/// </summary>
	/// <param name="cancellationToken"></param>
	[Get("/deployments/v2/policies")]
	Task<List<Policy>> ListPoliciesAsync(
		string? type = "dns",
		int? page = 1,
		int? limit = 100,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// List the Sites for the organization
	/// </summary>
	/// <param name="cancellationToken"></param>
	[Get("/deployments/v2/sites")]
	Task<List<Site>> ListSitesAsync(
		int? page = 1,
		int? limit = 100,
		CancellationToken cancellationToken = default);
}
