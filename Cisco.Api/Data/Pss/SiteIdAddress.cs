using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
	public class SiteIdAddress
	{
		/// <summary>
		/// Site id where the device resides.
		/// </summary>
		[XmlElement("siteID")]
		public string SiteID { get; set; } = null!;

		/// <summary>
		/// Name of the site where the device resides.
		/// </summary>
		[XmlElement("siteName")]
		public string SiteName { get; set; } = null!;

		/// <summary>
		/// First address line of the site.
		/// </summary>
		[XmlElement("addressLine1")]
		public string AddressLine1 { get; set; } = null!;

		/// <summary>
		/// Second address line of the site.
		/// </summary>
		[XmlElement("addressLine2")]
		public string AddressLine2 { get; set; } = null!;

		/// <summary>
		/// Third address line of the site.
		/// </summary>
		[XmlElement("addressLine3")]
		public string AddressLine3 { get; set; } = null!;

		/// <summary>
		/// Fourth address line of the site.
		/// </summary>
		[XmlElement("addressLine4")]
		public string AddressLine4 { get; set; } = null!;

		/// <summary>
		/// City where the site is located.
		/// </summary>
		[XmlElement("city")]
		public string City { get; set; } = null!;

		/// <summary>
		/// Postal code where the site is located.
		/// </summary>
		[XmlElement("postalCode")]
		public string PostalCode { get; set; } = null!;

		/// <summary>
		/// State where the site is located.
		/// </summary>
		[XmlElement("state")]
		public string State { get; set; } = null!;

		/// <summary>
		/// Province where the site is locate
		/// </summary>
		[XmlElement("province")]
		public string Province { get; set; } = null!;

		/// <summary>
		/// Country where the site is located.
		/// </summary>
		[XmlElement("country")]
		public string Country { get; set; } = null!;
	}
}