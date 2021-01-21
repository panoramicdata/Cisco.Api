using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	/// <summary>
	/// The PSIRT Device
	/// </summary>
	public class DevicePsirt
	{
		/// <summary>
		///  PSIRT id of the device.
		/// </summary>
		[XmlElement("psirtId")]
		public string PsirtId { get; set; } = null!;

		/// <summary>
		/// Match confidence factor for the PSIRT item.
		/// </summary>
		[XmlElement("matchConfidence")]
		public string MatchConfidence { get; set; } = null!;

		/// <summary>
		/// Match reason factor for the PSIRT item.
		/// </summary>
		[XmlElement("matchReason")]
		public string MatchReason { get; set; } = null!;

		/// <summary>
		/// Software type of the device in the PSIRT item.
		/// </summary>
		[XmlElement("softwareType")]
		public string SoftwareType { get; set; } = null!;

		/// <summary>
		///  Software version of the device in the PSIRT item.
		/// </summary>
		[XmlElement("softwareVersion")]
		public string? SoftwareVersion { get; set; }
	}
}