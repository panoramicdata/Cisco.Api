using System;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	/// <summary>
	/// The ContractDetail
	/// </summary>
	public class ContractDetail
	{
		/// <summary>
		/// Device’s contract number
		/// </summary>
		[XmlElement("contractNumber")]
		public string ContractNumber { get; set; } = null!;

		/// <summary>
		/// Identifies what level of service the contract has.
		/// </summary>
		[XmlElement("serviceLevel")]
		public string ServiceLevel { get; set; } = null!;

		/// <summary>
		/// Indicates the type of service level agreement (SLA)
		/// that is noted in the contract.
		/// </summary>
		[XmlElement("slaType")]
		public string SlaType { get; set; } = null!;

		/// <summary>
		/// States the start date of the contract.
		/// </summary>
		[XmlElement("contractStartDate")]
		public DateTime ContractStartDate { get; set; }

		/// <summary>
		/// States the end date of the contract.
		/// </summary>
		[XmlElement("contractEndDate")]
		public DateTime ContractEndDate { get; set; }

		/// <summary>
		/// Contact name of the vendor.
		/// </summary>
		[XmlElement("ContactNameOfVendor")]
		public string ContactNameOfVendor { get; set; } = null!;

		/// <summary>
		/// Id used when billing the site for the device.
		[XmlElement("BillToSiteID")]
		public string BillToSiteID { get; set; } = null!;
	}
}