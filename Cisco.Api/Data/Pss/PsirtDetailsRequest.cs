using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	/// <summary>
	/// The PsirtDetails Request
	/// </summary>
	[XmlRoot("PSIRTDetailsRequestInput", Namespace = "http://www.cisco.com/PSIRTAlertService")]
	public class PsirtDetailsRequest
	{
		/// <summary>
		/// Was obtained from the getPSIRTDetails API service call.
		/// </summary>
		[XmlArray("psirtIds")]
		[XmlArrayItem("psirtId")]
		public List<string> Ids { get; set; } = null!;
	}
}