using Cisco.Api.Data.Psirt;
using Refit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Interfaces;

/// <summary>
/// Psirt calls
/// </summary>
public interface IPsirt
{
	/// <summary>
	/// Used to obtain information about all published security advisories.
	/// </summary>
	/// <param name="cancellationToken"></param>
	[Get("/security/advisories/all")]
	Task<AdvisoriesResponse> GetAllAsync(
		CancellationToken cancellationToken);

	/// <summary>
	/// Using an absolute number to obtain the latest security advisories.
	/// </summary>
	/// <param name="count"></param>
	/// <param name="cancellationToken"></param>
	[Get("/security/advisories/latest/{count}")]
	Task<AdvisoriesResponse> GetLatestAsync(
		int count,
		CancellationToken cancellationToken);

	/// <summary>
	/// Used to obtain an advisory given its advisory ID (e.g., cisco-sa-20180221-ucdm )
	/// </summary>
	/// <param name="advisoryId"></param>
	/// <param name="cancellationToken"></param>
	[Get("/security/advisories/advisory/{advisoryId}")]
	Task<AdvisoriesResponse> GetByIdAsync(
		string advisoryId,
		CancellationToken cancellationToken);

	/// <summary>
	/// Used to obtain an advisory using a given Common Vulnerability Enumerator (CVE).
	/// </summary>
	/// <param name="cveId">Formatted as CVE-YYYY-NNNN.</param>
	/// <param name="cancellationToken"></param>
	[Get("/security/advisories/cve/{cveId}")]
	Task<AdvisoriesResponse> GetByCveIdAsync(
		string cveId,
		CancellationToken cancellationToken);

	/// <summary>
	/// Used to obtain all the advisories that affects the given product name.
	/// </summary>
	/// <param name="productKeyword"></param>
	/// <param name="cancellationToken"></param>
	[Get("/security/advisories/product/{productKeyword}")]
	Task<AdvisoriesResponse> GetAllByProductKeywordAsync(
		string productKeyword,
		CancellationToken cancellationToken);

	/// <summary>
	/// Used to obtain all security advisories published in a given year.
	/// </summary>
	/// <param name="year"></param>
	/// <param name="cancellationToken"></param>
	[Get("/security/advisories/year/{year}")]
	Task<AdvisoriesResponse> GetAllByPublishYearAsync(
		int year,
		CancellationToken cancellationToken);

	/// <summary>
	/// Used to obtain all security advisories for a given security impact rating (critical, high, medium, or low) and additionally filter based of firstpublished start and end date.
	/// </summary>
	/// <param name="sir"></param>
	/// <param name="startDate"></param>
	/// <param name="endDate"></param>
	/// <param name="cancellationToken"></param>
	[Get("/security/advisories/severity/{sir}/firstpublished")]
	Task<AdvisoriesResponse> GetAllByProductKeywordAsync(
		SecurityImpactRating sir,
		DateTimeOffset? startDate,
		DateTimeOffset? endDate,
		CancellationToken cancellationToken);

	/// <summary>
	/// This functionality allows you to query the Cisco Software Checker.
	/// This method is used to obtain Cisco Security Advisories that apply to specific Cisco IOS Software releases and have a Security Impact Rating(SIR) of Critical or High.
	/// </summary>
	/// <param name="version"></param>
	/// <param name="cancellationToken"></param>
	[Get("/security/advisories/ios")]
	Task<AdvisoriesResponse> GetAllByIosVersionAsync(
		string? version,
		CancellationToken cancellationToken);

	/// <summary>
	/// This functionality allows you to query the Cisco Software Checker.
	/// This method is used to obtain Cisco Security Advisories that apply to specific Cisco IOS-XE Software releases and have a Security Impact Rating(SIR) of Critical or High.
	/// </summary>
	/// <param name="version"></param>
	/// <param name="cancellationToken"></param>
	[Get("/security/advisories/iosxe")]
	Task<AdvisoriesResponse> GetAllByIosXeVersionAsync(
		string? version,
		CancellationToken cancellationToken);

	/// <summary>
	/// This functionality allows you to query the Cisco Software Checker.
	/// This method is used to obtain Cisco Security Advisories that apply to specific Cisco NX-OS (standalone) Software releases and have a Security Impact Rating(SIR) of Critical or High.
	/// </summary>
	/// <param name="version"></param>
	/// <param name="cancellationToken"></param>
	[Get("/security/advisories/nxos")]
	Task<AdvisoriesResponse> GetAllByNxosVersionAsync(
		string? version,
		CancellationToken cancellationToken);

	/// <summary>
	/// This functionality allows you to query the Cisco Software Checker.
	/// This method is used to obtain Cisco Security Advisories that apply to specific Cisco NX-OS (ACI mode) Software releases and have a Security Impact Rating (SIR) of Critical or High.
	/// </summary>
	/// <param name="version"></param>
	/// <param name="cancellationToken"></param>
	[Get("/security/advisories/aci")]
	Task<AdvisoriesResponse> GetAllByAciVersionAsync(
		string? version,
		CancellationToken cancellationToken);
}