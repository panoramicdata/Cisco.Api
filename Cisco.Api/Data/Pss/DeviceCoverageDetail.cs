using System.Collections.Generic;
using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

    /// <summary>
    /// The DeviceCoverageDetail
    /// </summary>
    public class DeviceCoverageDetail
    {
        /// <summary>
        /// Id of the device that we have the contract info for.
        /// </summary>
        [XmlElement("deviceId")]
        public string? DeviceId { get; set; }

        /// <summary>
        /// Serial number of the device.
        /// </summary>
        [XmlElement("serialNumber")]
        public string? SerialNumber { get; set; }

        /// <summary>
        /// Product id (PID) of the device.
        /// </summary>
        [XmlElement("productId")]
        public string? ProductId { get; set; }

        /// <summary>
        /// List of CoverageDetails
        /// </summary>
        [XmlElement("coverageDetails")]
        public List<CoverageDetail> CoverageDetails { get; set; } = [];
    }