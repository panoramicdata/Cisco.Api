using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.EnterpriseAgreement.Responses;

/// <summary>
/// Represents the subscription account.
/// </summary>
[DataContract]
public class SubscriptionAccount
{
	/// <summary>
	/// Gets or sets the smart account ID.
	/// </summary>
	[DataMember(Name = "smartAccountId")]
	public int SmartAccountId { get; set; }

	/// <summary>
	/// Gets or sets the smart account name.
	/// </summary>
	[DataMember(Name = "smartAccountName")]
	public string SmartAccountName { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the virtual accounts.
	/// </summary>
	[DataMember(Name = "vitualAccounts")]
	public List<VirtualAccount> VirtualAccounts { get; set; } = [];
}
