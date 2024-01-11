using Cisco.Api.Data.SoftwareSuggestion;
using Refit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Interfaces;

/// <summary>
/// Software Suggestion calls
/// </summary>
public interface ISoftwareSuggestion
{
	/// <summary>
	/// Gets software suggestion information by product IDs
	/// </summary>
	/// <param name="productIds">The product IDs</param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	/// <returns>The software suggestions</returns>
	[Get("/software/suggestion/v2/suggestions/software/productIds/{productIds}")]
	Task<SoftwareSuggestionPage> GetByProductIdsAsync(
		IEnumerable<string> productIds,
		int pageIndex = 1,
		CancellationToken cancellationToken = default);
}