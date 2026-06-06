using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.EnterpriseAgreement.Responses;

[DataContract]
public class VirtualAccount
{
	[DataMember(Name = "virtualAccountId")]
	public int VirtualAccountId { get; set; }

	[DataMember(Name = "virtualAccountName")]
	public string VirtualAccountName { get; set; } = string.Empty;

	[DataMember(Name = "suites")]
	public List<Suite> Suites { get; set; } = [];
}
