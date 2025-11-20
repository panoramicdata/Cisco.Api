using Cisco.Api.Data.Eox;
using Cisco.Api.Data.Pss;
using Refit;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Cisco.Api.Interfaces;

/// <summary>
/// EOX calls
/// </summary>
public interface IEox
{
	/// <summary>
	/// Returns the EoX record for products with the specified serial numbers. 
	/// </summary>
	/// <param name="serialNumber">Device serial number or numbers for which to return results. You can enter up to 20 serial numbers (each with a maximum length of 40) separated by commas.</param>
	/// <param name="pageIndex">Index number of the page to return; a maximum of 50 records per page are returned.</param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	/// <returns>The EOX information</returns>
	[Get("/supporttools/eox/rest/5/EOXBySerialNumber/{pageIndex}/{serialNumber}")]
	Task<EoxInfoPage> GetBySerialNumberAsync(
		[Body] string serialNumber,
		int pageIndex = 1,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Returns all active and inactive EoX records for all products with the specified eoxAttrib value within the startDate and endDate values, inclusive. If you do not specify an eoxAttrib value, this method returns records with an updated time stamp within the specified date range.
	/// Note: This method can be used to retrieve records based on any date listed in the EoX record. For example, if you specify a date range and enter EO_SALE_DATE and EO_LAST_SUPPORT_DATE as the eoxAttrib values, this method returns records with an end of sale date or last date of support within the specified date range.This feature allows you to target specific date ranges within each attribute without having to pull the entire database.
	/// </summary>
	/// <param name="startDate">Start date of the date range of records to return in the following format: YYYY-MM-DD. For example: 2010-01-01</param>
	/// <param name="endDate">End date of the date range of records to return in the following format: YYYY-MM-DD. For example: 2010-01-01</param>
	/// <param name="pageIndex">Index number of the page to return; a maximum of 50 records per page are returned.</param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	/// <returns>The EOX information</returns>
	[Get("/supporttools/eox/rest/5/EOXByDates/{pageIndex}/{startDate}/{endDate}")]
	Task<EoxInfoPage> GetByDatesAsync(
		[Query(Format = "yyyy-MM-dd")] DateTime startDate,
		[Query(Format = "yyyy-MM-dd")] DateTime endDate,
		int pageIndex = 1,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Returns one or more EOX records for the product or products with the specified product ID (PID) or product IDs.
	/// </summary>
	/// <param name="productId">Product IDs for the products to retrieve from the database. Enter up to 20 PIDs separated by commas. For example: 15216-OADM1-35=,M92S1K9-1.3.3C Note: To enhance search capabilities, the Cisco Support Tools allows wildcards with the productIDs parameter. A minimum of 3 characters is required. For example, only the following inputs are valid: *VPN*, *VPN, VPN*, and VPN. Using wildcards can result in multiple PIDs in the output.</param>
	/// <param name="pageIndex">Index number of the page to return; a maximum of 50 records per page are returned. </param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	/// <returns>The EOX information</returns>
	[Get("/supporttools/eox/rest/5/EOXByProductID/{pageIndex}/{productId}")]
	Task<EoxInfoPage> GetByProductIdAsync(
		string productId,
		int pageIndex = 1,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Returns the EoX record for products associated with the specified software release and (optionally) the specified operating system.
	/// </summary>
	/// <param name="softwareReleaseStrings">String for software release and type of operating system (optional) for the requested product. For example: 12.2,IOS You can enter up to 20 software release and operating system type combinations. Each combination can return multiple EoX records.</param>
	/// <param name="pageIndex">Index number of the page to return. For example, 1 returns the first page of the total number of available pages.</param>
	/// <param name="cancellationToken">An optional cancellation token</param>
	/// <returns>The EOX information</returns>
	[Get("/supporttools/eox/rest/5/EOXBySWReleaseString/{pageIndex}?input1={softwareReleaseStrings}")]
	Task<EoxInfoPage> GetBySoftwareReleaseStringAsync(
		IEnumerable<string> softwareReleaseStrings,
		int pageIndex = 1,
		CancellationToken cancellationToken = default);
}