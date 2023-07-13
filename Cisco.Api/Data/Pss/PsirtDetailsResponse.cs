using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	/// <summary>
	/// The PsirtDetails Response
	/// </summary>
	[XmlRoot("PSIRTDetailsResponseOutput", Namespace = "http://www.cisco.com/PSIRTAlertService")]
	public class PsirtDetailsResponse : PssServiceResponse
	{
		/// <summary>
		/// The List of PsirtDetails
		/// </summary>
		[XmlElement("PSIRTDetailsList")]
		public List<PsirtDetailsDTO> Details { get; set; } = new List<PsirtDetailsDTO>();
	}
}