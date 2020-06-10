using System;
using System.Collections.Generic;
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
		public async Task<EoxRecord> GetEoxInfoBySerialNumberAsync(string serialNumber, CancellationToken cancellationToken = default)
			=> (await GetAsync<EoxInfoPage>($"supporttools/eox/rest/5/EOXBySerialNumber/1/{serialNumber}", cancellationToken).ConfigureAwait(false)).EoxRecords.FirstOrDefault();

		/// <summary>
		/// Gets EOX information for multiple serial numbers
		/// </summary>
		/// <param name="serialNumbers">The serial numbers</param>
		/// <param name="cancellationToken">An optional cancellation token</param>
		/// <returns>A list of the EOX information</returns>
		public async Task<List<EoxRecord>> GetEoxInfoBySerialNumbersAsync(List<string> serialNumbers, CancellationToken cancellationToken = default)
			=> (await GetAsync<EoxInfoPage>($"supporttools/eox/rest/5/EOXBySerialNumber/1/{string.Join(",", serialNumbers)}", cancellationToken).ConfigureAwait(false)).EoxRecords;

		/// <summary>
		/// Gets EOX information for a date range
		/// </summary>
		/// <param name="startDate">The start date</param>
		/// <param name="endDate">The end date</param>
		/// <param name="pageIndex">The page index</param>
		/// <param name="cancellationToken">An optional cancellation token</param>
		/// <returns>The EOX information</returns>
		public async Task<EoxInfoPage> GetEoxInfoByDatesAsync(DateTime startDate, DateTime endDate, int pageIndex = 1, CancellationToken cancellationToken = default)
			=> await GetAsync<EoxInfoPage>($"supporttools/eox/rest/5/EOXByDates/{pageIndex}/{startDate:yyyy-MM-dd}/{endDate:yyyy-MM-dd}", cancellationToken).ConfigureAwait(false);

		/// <summary>
		/// Gets EOX information for a product id
		/// </summary>
		/// <param name="productId">The product id</param>
		/// <param name="pageIndex">The page index</param>
		/// <param name="cancellationToken">An optional cancellation token</param>
		/// <returns>The EOX information</returns>
		public async Task<EoxInfoPage> GetEoxInfoByProductIdAsync(string productId, int pageIndex = 1, CancellationToken cancellationToken = default)
			=> await GetAsync<EoxInfoPage>($"supporttools/eox/rest/5/EOXByProductID/{pageIndex}/{productId}", cancellationToken).ConfigureAwait(false);

		/// <summary>
		/// Gets EOX information for a single software release string
		/// </summary>
		/// <param name="softwareReleaseString">The software release string</param>
		/// <param name="pageIndex">The page index</param>
		/// <param name="cancellationToken">An optional cancellation token</param>
		/// <returns>The EOX information</returns>
		public async Task<EoxInfoPage> GetEoxInfoBySoftwareReleaseStringAsync(string softwareReleaseString, int pageIndex = 1, CancellationToken cancellationToken = default)
			=> await GetAsync<EoxInfoPage>($"supporttools/eox/rest/5/EOXBySWReleaseString/{pageIndex}?input1={softwareReleaseString}", cancellationToken).ConfigureAwait(false);

		/// <summary>
		/// Gets EOX information for multiple software release strings
		/// </summary>
		/// <param name="softwareReleasesStringList">The software release string e.g. 12.2,IOS (comma required)</param>
		/// <param name="pageIndex">The page index</param>
		/// <param name="cancellationToken">An optional cancellation token</param>
		/// <returns>The EOX information</returns>
		public async Task<EoxInfoPage> GetEoxInfoBySoftwareReleaseStringAsync(List<string> softwareReleasesStringList, int pageIndex = 1, CancellationToken cancellationToken = default)
			=> await GetAsync<EoxInfoPage>($"supporttools/eox/rest/5/EOXBySWReleaseString/{pageIndex}?{GenerateInputParameters(softwareReleasesStringList)}", cancellationToken).ConfigureAwait(false);

		private string GenerateInputParameters(List<string> strings)
			=> string.Join("&", strings.Select(s => $"input{strings.IndexOf(s) + 1}={s}"));
	}
}
