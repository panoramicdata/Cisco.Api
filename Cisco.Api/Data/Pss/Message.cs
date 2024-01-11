using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

    /// <summary>
    /// The same message format is used by different API’s
    /// and is where the error information is placed in the event of an ERROR;
    /// the actual content will vary for each different message.
    /// </summary>
    //[XmlRoot("message", Namespace = "http://www.cisco.com/InventoryService")]
    public class Message
    {
        [XmlElement("messageType")]
        public string? MessageType { get; set; }

        [XmlElement("messageDetail")]
        public string? MessageDetail { get; set; }
    }