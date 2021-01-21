using System.Runtime.Serialization;

namespace Cisco.Api.Data.SerialNumberToInfo
{
	/// <summary>
	/// Coverage status, warranty, and product identifier details
	/// </summary>
	[DataContract]
	public class CoverageSummary
	{
		/// <summary>
		/// For Future Use
		/// Base or manufacturing product identifiers related to the specified serial number.
		/// </summary>
		[DataMember(Name = "base_pid")]
		public string BasePid { get; set; } = null!;

		/// <summary>
		/// Address field for the contract install site; for example, 170 WEST TASMAN DR.
		/// </summary>
		[DataMember(Name = "contract_site_address1")]
		public string ContractSiteAddress1 { get; set; } = null!;

		/// <summary>
		/// City field for the contract install site; for example, SAN JOSE.
		/// </summary>
		[DataMember(Name = "contract_site_city")]
		public string ContractSiteCity { get; set; } = null!;

		/// <summary>
		/// Country field for the contract install site; for example, US.
		/// </summary>
		[DataMember(Name = "contract_site_country")]
		public string ContractSiteCountry { get; set; } = null!;

		/// <summary>
		/// Customer name associated to the contract install site; for example, CISCO SYSTEMS, INC..
		/// </summary>
		[DataMember(Name = "contract_site_customer_name")]
		public string ContractSiteCustomerName { get; set; } = null!;

		/// <summary>
		/// State field for the contract install site; for example, CA.
		/// </summary>
		[DataMember(Name = "contract_site_state_province")]
		public string ContractSiteStateProvince { get; set; } = null!;

		/// <summary>
		/// End date of the covered product line in the following format: YYYY-MM-DD; for example, 2010-01-01.
		/// </summary>
		[DataMember(Name = "covered_product_line_end_date")]
		public string CoveredProductLineEndDate { get; set; } = null!;

		/// <summary>
		/// Number of the record in the results.
		/// </summary>
		[DataMember(Name = "id")]
		public string Id { get; set; } = null!;

		/// <summary>
		/// Indicates whether the specified serial number is covered by a service contract; one of the following values: YES or NO. If the serial number is covered by a service contract, the value is Yes.
		/// </summary>
		[DataMember(Name = "is_covered")]
		public string IsCovered { get; set; } = null!;

		/// <summary>
		/// Indicates whether the specified serial number is covered by a service contract; one of the following values: YES or NO. If the serial number is covered by a service contract, the value is Yes.
		/// </summary>
		[DataMember(Name = "item_description")]
		public string ItemDescription { get; set; } = null!;

		/// <summary>
		/// Specifies whether the item specified by orderable_pid is MAJOR, MINOR, or STANDALONE; one of the following values:
		/// P = PARENT(MAJOR)
		/// C = CHILD(MINOR)
		/// S = STANDALONE
		/// </summary>
		[DataMember(Name = "item_position")]
		public string ItemPosition { get; set; } = null!;

		/// <summary>
		/// For Future Use
		/// Type of item identified by the orderable_pid value; for example, card or chassis.
		/// </summary>
		[DataMember(Name = "item_type")]
		public string ItemType { get; set; } = null!;

		/// <summary>
		/// Last page number.
		/// </summary>
		[DataMember(Name = "last_index")]
		public int LastIndex { get; set; }

		/// <summary>
		/// Orderable product identifiers for the specified serial number; for example, HWIC-4ESW.
		/// </summary>
		[DataMember(Name = "orderable_pid")]
		public string OrderablePid { get; set; } = null!;

		/// <summary>
		/// 	Current page number.
		/// </summary>
		[DataMember(Name = "page_index")]
		public int PageIndex { get; set; }

		/// <summary>
		/// Number of results per page returned.
		/// </summary>
		[DataMember(Name = "page_records")]
		public string PageRecordCount { get; set; } = null!;

		/// <summary>
		/// Parent serial number. The value of parent_sr_no will be the same as the value for sr_no if the item is a MAJOR item; that is, if item_position = P (Parent) or item_position = S (Stand Alone). The value of parent_sr_no will be different than the value for sr_no if the item is a MINOR item; that is, item_position = C (Child).
		/// </summary>
		[DataMember(Name = "parent_sr_no")]
		public string ParentSerialNumber { get; set; } = null!;

		/// <summary>
		/// Specifies the contract service line for the item specified by orderable_pid; one of the following values:
		/// 1 = TAC Support
		/// 2 = Hardware replacement
		/// 3 = Software Support
		/// A single service line can be associated to multiple pillars.
		/// </summary>
		[DataMember(Name = "pillar_code")]
		public string PillarCode { get; set; } = null!;

		/// <summary>
		/// Complete URI of the submitted request
		/// </summary>
		[DataMember(Name = "self_link")]
		public string SelfLink { get; set; } = null!;

		/// <summary>
		/// Service contract number; for example, 1234567, 2345678, 3456789.
		/// </summary>
		[DataMember(Name = "service_contract_number")]
		public string ServiceContractNumber { get; set; } = null!;

		/// <summary>
		/// Description of the service type; for example, SMARTnet Premium 24x7x4.
		/// </summary>
		[DataMember(Name = "service_line_descr")]
		public string ServiceLineDescription { get; set; } = null!;

		/// <summary>
		/// Serial number of the device.
		/// </summary>
		[DataMember(Name = "sr_no")]
		public string SerialNumber { get; set; } = null!;

		/// <summary>
		/// Title of the request.
		/// </summary>
		[DataMember(Name = "title")]
		public string Title { get; set; } = null!;

		/// <summary>
		/// Total number of records returned.
		/// </summary>
		[DataMember(Name = "total_records")]
		public string TotalRecordCount { get; set; } = null!;

		/// <summary>
		/// End date of the warranty for the specified serial number in the following format: YYYY-MM-DD; for example, 2010-01-01.
		/// </summary>
		[DataMember(Name = "warranty_end_date")]
		public string WarrantyEndDate { get; set; } = null!;

		/// <summary>
		/// Warranty service type; for example, WARR-3YR-HW-90D-SW.
		/// </summary>
		[DataMember(Name = "warranty_type")]
		public string WarrantyType { get; set; } = null!;

		/// <summary>
		/// Link to the description of the warranty type.
		/// </summary>
		[DataMember(Name = "warranty_type_description")]
		public string WarrantyTypeDescription { get; set; } = null!;
	}
}