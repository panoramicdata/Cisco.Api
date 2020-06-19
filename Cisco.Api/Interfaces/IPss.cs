using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Interfaces
{
	/// <summary>
	/// PSS calls
	/// </summary>
	public interface IPss
	{
		// TODO - REMOVE THIS ONE

		/// <summary>
		/// Say "hello" and get a response.
		/// </summary>
		/// <param name="cancellationToken">An optional cancellation token</param>
		/// <returns>The EOX information</returns>
		[Get("/hello")]
		Task<string> HelloAsync(
			CancellationToken cancellationToken = default);
	}
}