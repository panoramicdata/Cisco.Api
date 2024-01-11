using System.Runtime.Serialization;

namespace Cisco.Api.Data.Eox;

/// <summary>
/// An EOX record
/// </summary>
/// <seealso cref="https://developer.cisco.com/docs/support-apis/#!eox/eoxrecordtype"/>
[DataContract]
public class EoxRecord
{
	/// <summary>
	/// Product ID assigned to the product.
	/// </summary>
	[DataMember(Name = "EolProductID")]
	public string EolProductId { get; set; } = null!;

	/// <summary>
	/// Description of the product.
	/// </summary>
	[DataMember(Name = "ProductIDDescription")]
	public string ProductDescription { get; set; } = null!;

	/// <summary>
	/// Number assigned to the EoX product bulletin that announces the end of sale and end of life of the product to the general public.
	/// </summary>
	[DataMember(Name = "ProductBulletinNumber")]
	public string ProductBulletinNumber { get; set; } = null!;

	/// <summary>
	/// URL to the EoX product bulletin associated with the product.
	/// </summary>
	[DataMember(Name = "LinkToProductBulletinURL")]
	public string LinkToProductBulletinUrl { get; set; } = null!;

	/// <summary>
	/// Date that the end of sale and end of life of the product was announced to the general public.
	/// </summary>
	[DataMember(Name = "EoxExternalAnnouncementDate")]
	public CiscoDate ExternalAnnouncementDate { get; set; } = null!;

	/// <summary>
	/// Last date to order the requested product through Cisco point-of-sale mechanisms. The product is no longer for sale after this date.
	/// </summary>
	[DataMember(Name = "EndOfSaleDate")]
	public CiscoDate EndOfSaleDate { get; set; } = null!;

	/// <summary>
	/// Last date that Cisco Engineering might release any software maintenance releases or bug fixes to the software product. After this date, Cisco Engineering no longer develops, repairs, maintains, or tests the product software.
	/// </summary>
	[DataMember(Name = "EndOfSWMaintenanceReleases")]
	public CiscoDate EndOfSoftwareMaintenanceReleases { get; set; } = null!;

	/// <summary>
	/// Last date that Cisco Engineering may release a planned maintenance release or scheduled software remedy for a security vulnerability issue.
	/// </summary>
	[DataMember(Name = "EndOfSecurityVulSupportDate")]
	public CiscoDate EndOfSecurityVulnerabilitySupportDate { get; set; } = null!;

	/// <summary>
	/// Last date Cisco might perform a routine failure analysis to determine the root cause of an engineering-related or manufacturing-related issue.
	/// </summary>
	[DataMember(Name = "EndOfRoutineFailureAnalysisDate")]
	public CiscoDate EndOfRoutineFailureAnalysisDate { get; set; } = null!;

	/// <summary>
	/// Last date to extend or renew a service contract for the product. The extension or renewal period cannot extend beyond the last date of support.
	/// </summary>
	[DataMember(Name = "EndOfServiceContractRenewal")]
	public CiscoDate EndOfServiceContractRenewalDate { get; set; } = null!;

	/// <summary>
	/// Last date to receive service and support for the product. After this date, all support services for the product are unavailable, and the product becomes obsolete.
	/// </summary>
	[DataMember(Name = "LastDateOfSupport")]
	public CiscoDate LastSupportDate { get; set; } = null!;

	/// <summary>
	/// Last date to order a new service-and-support contract or add the equipment and/or software to an existing service-and-support contract for equipment and software that is not covered by a service-and-support contract.
	/// </summary>
	[DataMember(Name = "EndOfSvcAttachDate")]
	public CiscoDate EndOfServiceAttachDate { get; set; } = null!;

	/// <summary>
	/// Date the EoX record was created or last modified.
	/// </summary>
	[DataMember(Name = "UpdatedTimestamp")]
	public CiscoDate UpdatedDate { get; set; } = null!;

	/// <summary>
	/// Information that describes an error (if any) received during the operation.
	/// </summary>
	[DataMember(Name = "EOXError")]
	public EoxError? Error { get; set; }

	/// <summary>
	/// Information regarding migration of the EoX product.
	/// </summary>
	[DataMember(Name = "EOXMigrationDetails")]
	public EoxMigrationDetails EoxMigrationDetails { get; set; } = null!;

	/// <summary>
	/// Method name of the request sent for this operation.
	/// </summary>
	[DataMember(Name = "EoxInputType")]
	public string InputType { get; set; } = null!;

	/// <summary>
	/// Request parameters sent for this operation.
	/// </summary>
	[DataMember(Name = "EoxInputValue")]
	public string InputValue { get; set; } = null!;
}