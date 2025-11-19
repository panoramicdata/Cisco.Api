using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.EnterpriseAgreement.Responses;

[DataContract]
public class SubscriptionAccount
{
	[DataMember(Name = "smartAccountId")]
	public int SmartAccountId { get; set; }

	[DataMember(Name = "smartAccountName")]
	public string SmartAccountName { get; set; } = string.Empty;

	[DataMember(Name = "vitualAccounts")]
	public List<VirtualAccount> VirtualAccounts { get; set; } = [];
}
