using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	public class SoftwareEoxDevice
	{
		/// <summary>
		/// Software end-of-life id.
		/// </summary>
		[XmlElement("softwareEoxId")]
		public string SoftwareEoxId { get; set; } = null!;

		/// <summary>
		/// Software version number.
		/// </summary>
		[XmlElement("softwareVersion")]
		public string SoftwareVersion { get; set; } = null!;

		/// <summary>
		/// Indicates the type of software.
		/// </summary>
		[XmlElement("softwareType")]
		public string SoftwareType { get; set; } = null!;
	}
}