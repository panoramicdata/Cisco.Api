using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api
{
	public partial class CiscoClient
	{
		/// <summary>
		/// Gets EOX information for a single serial number
		/// </summary>
		/// <param name="serialNumber">The serial number</param>
		/// <param name="cancellationToken">An optional cancellation token</param>
		/// <returns>The EOX information</returns>
		public async Task<EoxRecord> GetEoxInfoBySerialNumberAsync(string serialNumber, CancellationToken? cancellationToken = null)
			=> (await GetAsync<EoxInfoPage>($"supporttools/eox/rest/5/EOXBySerialNumber/1/{serialNumber}", cancellationToken)).EoxRecord.FirstOrDefault();

		/// <summary>
		/// Gets EOX information for a date range
		/// </summary>
		/// <param name="startDate">The start date</param>
		/// <param name="endDate">The end date</param>
		/// <param name="pageIndex">The page index</param>
		/// <param name="cancellationToken">An optional cancellation token</param>
		/// <returns>The EOX information</returns>
		public async Task<EoxInfoPage> GetEoxInfoByDatesAsync(DateTime startDate, DateTime endDate, int pageIndex = 1, CancellationToken? cancellationToken = null)
			=> await GetAsync<EoxInfoPage>($"supporttools/eox/rest/5/EOXByDates/{pageIndex}/{startDate:yyyy-MM-dd}/{endDate:yyyy-MM-dd}", cancellationToken);

		/// <summary>
		/// Gets EOX information for a product id
		/// </summary>
		/// <param name="productId">The product id</param>
		/// <param name="pageIndex">The page index</param>
		/// <param name="cancellationToken">An optional cancellation token</param>
		/// <returns>The EOX information</returns>
		public async Task<EoxInfoPage> GetEoxInfoByProductIdAsync(string productId, int pageIndex = 1, CancellationToken? cancellationToken = null)
			=> await GetAsync<EoxInfoPage>($"supporttools/eox/rest/5/EOXByProductID/{pageIndex}/{productId}", cancellationToken);

		/// <summary>
		/// Gets EOX information for a single serial number
		/// </summary>
		/// <param name="softwareReleaseString">The software release string</param>
		/// <param name="pageIndex">The page index</param>
		/// <param name="cancellationToken">An optional cancellation token</param>
		/// <returns>The EOX information</returns>
		public async Task<EoxInfoPage> GetEoxInfoBySoftwareReleaseStringAsync(string softwareReleaseString, int pageIndex = 1, CancellationToken? cancellationToken = null)
			=> await GetAsync<EoxInfoPage>($"supporttools/eox/rest/5/EOXBySWReleaseString/{pageIndex}?input1={softwareReleaseString}", cancellationToken);
	}
}
