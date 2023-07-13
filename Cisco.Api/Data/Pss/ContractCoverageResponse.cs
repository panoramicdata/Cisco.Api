using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	/// <summary>
	/// The Contract Coverage response
	/// </summary>
	[XmlRoot("ContractCoverageResponse", Namespace = "http://www.cisco.com/ContractService")]
	public class ContractCoverageResponse : PssServiceResponse
	{
		/// <summary>
		/// A list of DeviceCoverageDetails
		/// </summary>
		[XmlElement("deviceCoverageDetail")]
		public List<DeviceCoverageDetail> DeviceCoverageDetails { get; set; } = null!;


		/// <summary>
		/// A list of ContractDetails
		/// </summary>
		[XmlElement("contractDetail")]
		public List<ContractDetail> ContractDetails { get; set; } = null!;

		/// <summary>
		/// A list of SiteIdAddress
		/// </summary>
		[XmlElement("siteIdAddress")]
		public List<SiteIdAddress> SiteIdAddresses { get; set; } = null!;
	}
}