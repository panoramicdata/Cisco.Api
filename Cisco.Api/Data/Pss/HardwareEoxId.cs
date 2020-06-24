using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	public class HardwareEoxId
	{
		/// <summary>
		/// Is the hardware end-of-life id and can have
		/// any number of parameters listed in the API service call.
		/// </summary>
		[XmlElement("hwEoxId")]
		public string Id { get; set; } = null!;
	}
}