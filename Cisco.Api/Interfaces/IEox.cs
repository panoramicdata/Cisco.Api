using Cisco.Api.Data.Eox;
using Refit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Interfaces
{
	/// <summary>
	/// EOX calls
	/// </summary>
	public interface IEox
	{
		/// <summary>
		/// Get EOX info by serial number
		/// </summary>
		/// <param name="serialNumber">The serial number</param>
		/// <param name="cancellationToken">An optional cancellation token</param>
		/// <returns>The EOX information</returns>
		[Get("/supporttools/eox/rest/5/EOXBySerialNumber/1/{serialNumber}")]
		Task<EoxInfoPage> GetBySerialNumberAsync(
			[Body] string serialNumber,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Gets EOX information for a date range
		/// </summary>
		/// <param name="startDate">The start date</param>
		/// <param name="endDate">The end date</param>
		/// <param name="pageIndex">The page index</param>
		/// <param name="cancellationToken">An optional cancellation token</param>
		/// <returns>The EOX information</returns>
		[Get("/supporttools/eox/rest/5/EOXByDates/{pageIndex}/{startDate}/{endDate}")]
		Task<EoxInfoPage> GetByDatesAsync(
			[Query(Format = "yyyy-MM-dd")] DateTime startDate,
			[Query(Format = "yyyy-MM-dd")] DateTime endDate,
			int pageIndex = 1,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Gets EOX information for a product id
		/// </summary>
		/// <param name="productId">The product id</param>
		/// <param name="pageIndex">The page index</param>
		/// <param name="cancellationToken">An optional cancellation token</param>
		/// <returns>The EOX information</returns>
		[Get("/supporttools/eox/rest/5/EOXByProductID/{pageIndex}/{productId}")]
		Task<EoxInfoPage> GetByProductIdAsync(
			string productId,
			int pageIndex = 1,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Gets EOX information for software release strings
		/// </summary>
		/// <param name="softwareReleaseStrings">The software release strings</param>
		/// <param name="pageIndex">The page index</param>
		/// <param name="cancellationToken">An optional cancellation token</param>
		/// <returns>The EOX information</returns>
		[Get("/supporttools/eox/rest/5/EOXBySWReleaseString/{pageIndex}?input1={softwareReleaseStrings}")]
		Task<EoxInfoPage> GetBySoftwareReleaseStringAsync(
			IEnumerable<string> softwareReleaseStrings,
			int pageIndex = 1,
			CancellationToken cancellationToken = default);
	}
}