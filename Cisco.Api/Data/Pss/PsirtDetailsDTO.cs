using System;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	/// <summary>
	/// The ResponseDTO when getting PsirtDetails
	/// </summary>
	public class PsirtDetailsDTO
	{
		/// <summary>
		/// PSIRT id of the PSIRT item.
		/// </summary>
		[XmlElement("psirtId")]
		public string PsirtId { get; set; } = null!;

		/// <summary>
		/// Type of distribution for the PSIRT.
		/// </summary>
		[XmlElement("distributionType")]
		public string DistributionType { get; set; } = null!;

		/// <summary>
		/// Document number for the PSIRT.
		/// </summary>
		[XmlElement("documentNumber")]
		public string DocumentNumber { get; set; } = null!;

		/// <summary>
		/// External url for the PSIRT.
		/// </summary>
		[XmlElement("externalURL")]
		public string ExternalURL { get; set; } = null!;

		/// <summary>
		/// Date the PSIRT was first published.
		/// </summary>
		[XmlElement("firstPublished")]
		public DateTime FirstPublished { get; set; }

		/// <summary>
		/// Headline for the PSIRT.
		/// </summary>
		[XmlElement("headline")]
		public string Headline { get; set; } = null!;

		/// <summary>
		/// Date the PSIRT was last updated.
		/// </summary>
		[XmlElement("lastUpdated")]
		public DateTime LastUpdated { get; set; }

		/// <summary>
		/// Revision number for the PSIRT.
		/// </summary>
		[XmlElement("revisionNumber")]
		public string RevisionNumber { get; set; } = null!;
	}
}