using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
    /// <summary>
    /// The SiteIdAddress
    /// </summary>
    public class SiteIdAddress
    {
        /// <summary>
        /// Site id where the device resides.
        /// </summary>
        [XmlElement("siteID")]
        public string? SiteID { get; set; }

        /// <summary>
        /// Name of the site where the device resides.
        /// </summary>
        [XmlElement("siteName")]
        public string? SiteName { get; set; }

        /// <summary>
        /// First address line of the site.
        /// </summary>
        [XmlElement("addressLine1")]
        public string? AddressLine1 { get; set; }

        /// <summary>
        /// Second address line of the site.
        /// </summary>
        [XmlElement("addressLine2")]
        public string? AddressLine2 { get; set; }

        /// <summary>
        /// Third address line of the site.
        /// </summary>
        [XmlElement("addressLine3")]
        public string? AddressLine3 { get; set; }

        /// <summary>
        /// Fourth address line of the site.
        /// </summary>
        [XmlElement("addressLine4")]
        public string? AddressLine4 { get; set; }

        /// <summary>
        /// City where the site is located.
        /// </summary>
        [XmlElement("city")]
        public string? City { get; set; }

        /// <summary>
        /// Postal code where the site is located.
        /// </summary>
        [XmlElement("postalCode")]
        public string? PostalCode { get; set; }

        /// <summary>
        /// State where the site is located.
        /// </summary>
        [XmlElement("state")]
        public string? State { get; set; }

        /// <summary>
        /// Province where the site is locate
        /// </summary>
        [XmlElement("province")]
        public string? Province { get; set; }

        /// <summary>
        /// Country where the site is located.
        /// </summary>
        [XmlElement("country")]
        public string? Country { get; set; }
    }
}