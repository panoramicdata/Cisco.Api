using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api
{
	[DataContract]
	public class CoverageSummary
	{
		[DataMember(Name= "base_pid_list")]
		public List<BasePidList> BasePidList { get; set; }

		[DataMember(Name = "contract_site_customer_name")]
		public string ContractSiteCustomerName { get; set; }

		[DataMember(Name = "contract_site_address1")]
		public string ContractSiteAddress1 { get; set; }

		[DataMember(Name = "contract_site_city")]
		public string ContractSiteCity { get; set; }

		[DataMember(Name = "contract_site_state_province")]
		public string ContractSiteStateProvince { get; set; }

		[DataMember(Name = "contract_site_country")]
		public string ContractSiteCountry { get; set; }

		[DataMember(Name = "covered_product_line_end_date")]
		public string CoveredProductLineEndDate { get; set; }

		[DataMember(Name = "id")]
		public string Id { get; set; }

		[DataMember(Name = "is_covered")]
		public string IsCovered { get; set; }

		[DataMember(Name = "orderable_pid_list")]
		public List<OrderablePidList> OrderablePidList { get; set; }

		[DataMember(Name = "parent_sr_no")]
		public string ParentSerialNumber { get; set; }

		[DataMember(Name = "service_contract_number")]
		public string ServiceContractNumber { get; set; }

		[DataMember(Name = "service_line_descr")]
		public string ServiceLineDescr { get; set; }

		[DataMember(Name = "sr_no")]
		public string SerialNumber { get; set; }

		[DataMember(Name = "warranty_end_date")]
		public string WarrantyEndDate { get; set; }

		[DataMember(Name = "warranty_type")]
		public string WarrantyType { get; set; }

		[DataMember(Name = "warranty_type_description")]
		public string WarrantyTypeDescription { get; set; }
	}
}