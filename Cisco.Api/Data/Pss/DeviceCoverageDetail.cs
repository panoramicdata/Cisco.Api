using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	public class DeviceCoverageDetail
	{
		/// <summary>
		/// Id of the device that we have the contract info for.
		/// </summary>
		[XmlElement("deviceId")]
		public string DeviceId { get; set; } = null!;

		/// <summary>
		/// Serial number of the device.
		/// </summary>
		[XmlElement("serialNumber")]
		public string SerialNumber { get; set; } = null!;

		/// <summary>
		/// Product id (PID) of the device.
		/// </summary>
		[XmlElement("productId")]
		public string ProductId { get; set; } = null!;

		/// <summary>
		/// CoverageDetail
		/// </summary>
		[XmlElement("coverageDetails")]
		public CoverageDetail CoverageDetail { get; set; } = null!;

		/// <summary>
		/// ContractDetail
		/// </summary>
		[XmlElement("contractDetail")]
		public ContractDetail ContractDetail { get; set; } = null!;

		/// <summary>
		/// SiteIdAddress
		/// </summary>
		[XmlElement("siteIdAddress")]
		public SiteIdAddress SiteIdAddress { get; set; } = null!;
	}
}