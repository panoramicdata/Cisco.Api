﻿using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	[XmlRoot("ContractCoverageResponse", Namespace = "http://www.cisco.com/ContractService")]
	public class ContractCoverageResponse : PssServiceResponse
	{
		/// <summary>
		/// A list of DeviceCoverageDetails
		/// </summary>
		[XmlElement("deviceCoverageDetail")]
		public List<DeviceCoverageDetail> DeviceCoverageDetails { get; set; } = null!;
	}
}