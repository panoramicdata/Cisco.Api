using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.EnterpriseAgreement.Responses;

/// <summary>
/// Represents the virtual account.
/// </summary>
[DataContract]
public class VirtualAccount
{
	/// <summary>
	/// Gets or sets the virtual account ID.
	/// </summary>
	[DataMember(Name = "virtualAccountId")]
	public int VirtualAccountId { get; set; }

	/// <summary>
	/// Gets or sets the virtual account name.
	/// </summary>
	[DataMember(Name = "virtualAccountName")]
	public string VirtualAccountName { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the suites.
	/// </summary>
	[DataMember(Name = "suites")]
	public List<Suite> Suites { get; set; } = [];
}
