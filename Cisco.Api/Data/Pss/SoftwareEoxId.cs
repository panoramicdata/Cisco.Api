using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	/// <summary>
	/// The SoftwareEoxId
	/// </summary>
	public class SoftwareEoxId
	{
		/// <summary>
		/// Was obtained from the getSoftwareEoX API service call.
		/// </summary>
		[XmlElement("softwareEoxId")]
		public string Id { get; set; } = null!;
	}
}