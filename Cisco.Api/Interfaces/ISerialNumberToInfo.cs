using Cisco.Api.Data.SerialNumberToInfo;
using Refit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Interfaces;

/// <summary>
/// Serial number to info calls
/// </summary>
public interface ISerialNumberToInfo
{
	/// <summary>
	/// Gets coverage status by serial numbers
	/// </summary>
	/// <param name="serialNumbers">The serial numbers</param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	/// <returns>The coverage status</returns>
	[Get("/sn2info/v2/coverage/status/serial_numbers/{serialNumbers}")]
	Task<CoverageStatusCollection> GetCoverageStatusBySerialNumbersAsync(
		IEnumerable<string> serialNumbers,
		CancellationToken cancellationToken);

	Task<CoverageStatusCollection> GetCoverageStatusBySerialNumbersAsync(IEnumerable<string> serialNumbers)
		=> GetCoverageStatusBySerialNumbersAsync(serialNumbers, default);

	/// <summary>
	/// Gets coverage summary by serial numbers
	/// </summary>
	/// <param name="serialNumbers">The serial numbers</param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	/// <returns>The coverage status</returns>
	[Get("/sn2info/v2/coverage/status/serial_numbers/{serialNumbers}")]
	Task<CoverageSummaryCollection> GetCoverageSummaryBySerialNumbersAsync(
		IEnumerable<string> serialNumbers,
		CancellationToken cancellationToken);

	Task<CoverageSummaryCollection> GetCoverageSummaryBySerialNumbersAsync(IEnumerable<string> serialNumbers)
		=> GetCoverageSummaryBySerialNumbersAsync(serialNumbers, default);

	/// <summary>
	/// Gets coverage summary by serial numbers
	/// </summary>
	/// <param name="serialNumbers">The serial numbers</param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	/// <returns>The coverage status</returns>
	[Get("/sn2info/v2/coverage/status/serial_numbers/{serialNumbers}")]
	Task<OrderablePidCollection> GetOrderableProductIdentifiersBySerialNumbersAsync(
		IEnumerable<string> serialNumbers,
		CancellationToken cancellationToken);

	Task<OrderablePidCollection> GetOrderableProductIdentifiersBySerialNumbersAsync(IEnumerable<string> serialNumbers)
		=> GetOrderableProductIdentifiersBySerialNumbersAsync(serialNumbers, default);
}