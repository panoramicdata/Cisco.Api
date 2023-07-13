using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
    public class HardwareEoxIds
    {
        /// <summary>
        /// The list od hardware end-of-life ids and can have
        /// any number of parameters listed in the API service call.
        /// </summary>
        [XmlElement("hwEoxId")]
        public List<string> Ids { get; set; } = new List<string>();
    }
}