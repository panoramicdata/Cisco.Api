using System.Xml.Serialization;

namespace Cisco.Api.Data.Pss;

/// <summary>
/// The Contract Coverage request
/// </summary>
[XmlRoot("ContractCoverageRequest", Namespace = "http://www.cisco.com/ContractService")]
public class ContractCoverageRequest : PssServiceRequest
{
}