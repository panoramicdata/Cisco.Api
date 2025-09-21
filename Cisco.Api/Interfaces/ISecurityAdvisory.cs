using Cisco.Api.Data.SecurityAdvisories;
using Refit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Interfaces
{
	/// <summary>
	/// Security calls
	/// </summary>
	public interface ISecurityAdvisory
	{
		/// <summary>
		/// Get a security advisory by ID
		/// </summary>
		/// <param name="id">The advisory ID</param>
		/// <param name="cancellationToken">An optional CancellationToken</param>
		/// <returns>The security advisory</returns>
		[Get("/security/advisories/v2/advisory/{id}")]
		Task<SecurityAdvisories> GetById(
			string id,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Get security advisories by type and version
		/// </summary>
		/// <param name="type">The type (such as asa or ftd)</param>
		/// <param name="version">The version e.g. 9.8.2</param>
		/// <param name="alias">The alias e.g. ASAV</param>
		/// <param name="cancellationToken">An optional CancellationToken</param>
		/// <returns>The security advisories</returns>
		[Get("/security/advisories/v2/OSType/{type}?version={version}&platformAlias={alias}")]
		Task<SecurityAdvisories> GetAdvisoriesByOsTypeAndVersion(
			string type,
			string version,
			string alias = "",
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Get security advisories by CVE name
		/// </summary>
		/// <param name="cvename">The CCE name, e.g. CVE-2022-20623</param>
		/// <param name="cancellationToken">An optional CancellationToken</param>
		/// <returns>The security advisories</returns>
		[Get("/security/advisories/v2/cve/{cvename}")]
		Task<SecurityAdvisories> GetAdvisoriesByCveName(
			string cvename,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Get OS version data by OS type
		/// </summary>
		/// <param name="osType">The OS type, e.g. nxos or ios</param>
		/// <param name="cancellationToken">An optional CancellationToken</param>
		/// <returns>The security advisories</returns>
		[Get("/security/advisories/v2/OS_version/OS_data?OSType={osType}")]
		Task<List<OsVersion>> GetOsVersionDataByType(
			string osType,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Get all security advisories; consider using paging for performance reasons.
		/// </summary>
		/// <param name="summaryDetails">Include the Advisory Summary Description or not. Setting to False can reduce the response size significantly.</param>
		/// <param name="productNames">Include the ProductNames or not. Setting to False can reduce the response size significantly.</param>
		/// <param name="cancellationToken">An optional CancellationToken</param>
		/// <returns>The security advisories</returns>
		[Get("/security/advisories/v2/all?summaryDetails={summaryDetails}&productNames={productNames}")]
		Task<SecurityAdvisories> GetAllAdvisories(
			bool summaryDetails = true,
			bool productNames = true,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Get all security advisories by page size and index
		/// </summary>
		/// <param name="pageSize">The pageIndex field is an integer representing the current page index out of total number of pages. The maximum pageIndex is 100.</param>
		/// <param name="pageIndex">The pageSize field is an integer representing the maximum number of items requested by the client for the current page. The maximum pageSize limit is 100.</param>
		/// <param name="summaryDetails">Include the Advisory Summary Description or not. Setting to False can reduce the response size significantly.</param>
		/// <param name="productNames">Include the ProductNames or not. Setting to False can reduce the response size significantly.</param>
		/// <param name="cancellationToken">An optional CancellationToken</param>
		/// <returns>The security advisories</returns>
		[Get("/security/advisories/v2/all?summaryDetails={summaryDetails}&productNames={productNames}&pageSize={pageSize}&pageIndex={pageIndex}")]
		Task<SecurityAdvisories> GetAllAdvisories(
			int pageSize,
			int pageIndex,
			bool summaryDetails = true,
			bool productNames = true,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Get latest security advisories, limited to the count provided
		/// </summary>
		/// <param name="count">The number you want returned</param>
		/// <param name="cancellationToken">An optional CancellationToken</param>
		/// <returns>The security advisories</returns>
		[Get("/security/advisories/v2/latest/{count}")]
		Task<SecurityAdvisories> GetLatesAdvisoriesLimitedByCount(
			int count,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Use the base advisories URL and provide a custom URL for maximum flexibility
		/// </summary>
		/// <param name="requestString">The request string</param>
		/// <param name="cancellationToken">An optional CancellationToken</param>
		/// <returns></returns>
		[Get("/security/advisories/v2/{customRequest}")]
		[QueryUriFormat(UriFormat.Unescaped)]
		Task<SecurityAdvisories> GetAdvisoriesByCustomRequest(
			string customRequest,
			CancellationToken cancellationToken = default);

		/// <summary>
		/// Get security advisories by type and version
		/// </summary>
		/// <param name="product">The product (such as Cisco)</param>
		/// <param name="cancellationToken">An optional CancellationToken</param>
		/// <returns>The security advisories</returns>
		[Get("/security/advisories/v2/product/?product={product}")]
		Task<SecurityAdvisories> GetAdvisoriesByProduct(
			string product,
			CancellationToken cancellationToken = default);
	}
}