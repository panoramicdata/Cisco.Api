using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	[XmlRoot("ContractCoverageResponse", Namespace = "http://www.cisco.com/ContractService")]
	public class ContractCoverageResponse
	{
		/// <summary>
		/// A list of DeviceCoverageDetails
		/// </summary>
		[XmlElement("deviceCoverageDetail")]
		public List<DeviceCoverageDetail> DeviceCoverageDetails { get; set; } = null!;


		/// <summary>
		/// Time stamp that indicates when this service call was performed.
		/// </summary>
		[XmlElement("responseTimestamp")]
		public DateTime ResponseTimestamp { get; set; }

		/// <summary>
		/// The same message format is used by different API’s and is
		/// where the error information is placed in the
		/// event of an ERROR; the actual content will vary for each different message.
		/// </summary>
		[XmlElement("message")]
		public Message Message { get; set; } = null!;


	}
}