using System;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss
{
    /// <summary>
    /// The CoverageDetail element
    /// </summary>
    public class CoverageDetail
    {
        /// <summary>
        /// Contract number associated to the selected device.
        /// </summary>
        [XmlElement("contractNumber")]
        public string? ContractNumber { get; set; }

        /// <summary>
        /// Status of the current coverage.
        /// </summary>
        [XmlElement("coverageStatus")]
        public string? CoverageStatus { get; set; }

        /// <summary>
        /// Indicates the type of service level agreement (SLA)
        /// that is noted in the contract.
        /// </summary>
        [XmlElement("slaType")]
        public string? SlaType { get; set; }

        /// <summary>
        /// Indicates what service level is active.
        /// </summary>
        [XmlElement("serviceLevel")]
        public string? ServiceLevel { get; set; }

        /// <summary>
        /// Indicates when the contract coverage started
        /// </summary>
        [XmlElement("coverageStartDate")]
        public DateTime? CoverageStartDate { get; set; }

        /// <summary>
        /// Indicates when the contract coverage will end.
        /// </summary>
        [XmlElement("coverageEndDate")]
        public DateTime? CoverageEndDate { get; set; }

        /// <summary>
        /// Ship date of the device.
        /// </summary>
        [XmlElement("shipDate")]
        public DateTime? ShipDate { get; set; }

        /// <summary>
        /// EOS date for the device.
        /// </summary>
        [XmlElement("eosDate")]
        public DateTime? EosDate { get; set; }

        /// <summary>
        /// This parameter indicates whether or not
        /// a PID or serial number is serviceable,
        /// or is covered under a parent contract.
        /// </summary>
        [XmlElement("serviceableFlag")]
        public string? ServiceableFlag { get; set; }

        /// <summary>
        /// Customer site id to where the device was shipped.
        /// </summary>
        [XmlElement("shipToSiteID")]
        public string? ShipToSiteID { get; set; }

        /// <summary>
        /// Id of the install site, where the device was installed.
        /// </summary>
        [XmlElement("installAtSiteID")]
        public string? InstallAtSiteID { get; set; }
    }
}