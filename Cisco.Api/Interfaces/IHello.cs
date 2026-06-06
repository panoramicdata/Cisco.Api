using Cisco.Api.Data.Hello;
using Refit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Interfaces;

/// <summary>
/// Hello call
/// </summary>
public interface IHello
{
	/// <summary>
	/// Say "hello" and get a response.
	/// </summary>
	/// <param name="cancellationToken">An optional cancellation token</param>
	/// <returns>The EOX information</returns>
	[Get("/hello")]
	Task<Response> HelloAsync(
		CancellationToken cancellationToken);

	[Obsolete("Pass a CancellationToken; for example: default. This overload will be removed in a future version.", true)]
	Task<Response> HelloAsync()
		=> HelloAsync(default);
}