using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

    /// <summary>
    /// The list of SoftwareEoxIds
    /// </summary>
    public class SoftwareEoxIds
    {
        /// <summary>
        /// Was obtained from the getSoftwareEoX API service call.
        /// </summary>
        [XmlElement("softwareEoxId")]
        public List<string> Ids { get; set; } = new List<string>();
    }