using Cisco.Api.Data.SoftwareSuggestion;
using Refit;
using System;
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
	/// <param name="pageIndex">The zero-based page index.</param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	/// <returns>The software suggestions</returns>
	[Get("/software/suggestion/v2/suggestions/software/productIds/{productIds}")]
	Task<SoftwareSuggestionPage> GetByProductIdsAsync(
		IEnumerable<string> productIds,
		int pageIndex,
		CancellationToken cancellationToken);

	/// <summary>
	/// Performs the get by product ids operation.
	/// </summary>
	[Obsolete("Pass a CancellationToken; for example: default. This overload will be removed in a future version.", true)]
	Task<SoftwareSuggestionPage> GetByProductIdsAsync(IEnumerable<string> productIds, int pageIndex)
		=> GetByProductIdsAsync(productIds, pageIndex, default);

	/// <summary>
	/// Performs the get by product ids operation.
	/// </summary>
	[Obsolete("Pass a CancellationToken; for example: default. This overload will be removed in a future version.", true)]
	Task<SoftwareSuggestionPage> GetByProductIdsAsync(IEnumerable<string> productIds)
		=> GetByProductIdsAsync(productIds, 1, default);
}