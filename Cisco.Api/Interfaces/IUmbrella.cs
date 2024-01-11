using Cisco.Api.Data.Umbrella;
using Refit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Interfaces;

public interface IUmbrella
{
	/// <summary>
	/// Used to obtain information about all published security advisories.
	/// </summary>
	/// <param name="cancellationToken"></param>
	[Get("/deployments/v2/internalnetworks")]
	Task<List<InternalNetwork>> GetAllInternalNetworksAsync(
		CancellationToken cancellationToken);
}
