using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api
{
	public partial class CiscoClient
	{
		/// <summary>
		/// Gets coverage status for a single serial number
		/// </summary>
		/// <param name="serialNumber">The serial number</param>
		/// <param name="cancellationToken">An optional cancellation token</param>
		/// <returns>The coverage status</returns>
		public async Task<CoverageStatus> GetCoverageStatusBySerialNumber(string serialNumber, CancellationToken? cancellationToken = null)
			=> (await GetAsync<CoverageStatusCollection>($"sn2info/v2/coverage/status/serial_numbers/{serialNumber}", cancellationToken).ConfigureAwait(false)).CoverageStatuses.FirstOrDefault();

		/// <summary>
		/// Gets coverage summary for a single serial number
		/// </summary>
		/// <param name="serialNumber">The serial number</param>
		/// <param name="cancellationToken">An optional cancellation token</param>
		/// <returns>The coverage summary</returns>
		public async Task<CoverageSummary> GetCoverageSummaryBySerialNumber(string serialNumber, CancellationToken? cancellationToken = null)
			=> (await GetAsync<CoverageSummaryCollection>($"sn2info/v2/coverage/summary/serial_numbers/{serialNumber}", cancellationToken).ConfigureAwait(false)).CoverageSummaries.FirstOrDefault();

		/// <summary>
		/// Gets orderable product identifier for a single serial number
		/// </summary>
		/// <param name="serialNumber">The serial number</param>
		/// <param name="cancellationToken">An optional cancellation token</param>
		/// <returns>The coverage summary</returns>
		public async Task<List<SerialNumberOrderablePid>> GetOrderableProductIdentifiersBySerialNumber(string serialNumber, CancellationToken? cancellationToken = null)
			=> (await GetAsync<OrderablePidCollection>($"sn2info/v2/identifiers/orderable/serial_numbers/{serialNumber}", cancellationToken).ConfigureAwait(false)).SerialNumberOrderablePids.FirstOrDefault()?.OrderablePids;
	}
}
