using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.EnterpriseAgreement.Responses;

/// <summary>
/// Represents the suite.
/// </summary>
[DataContract]
public class Suite : SuiteEntitlements
{
	/// <summary>
	/// Gets or sets the commerce skus.
	/// </summary>
	[DataMember(Name = "commerceSkus")]
	public List<CommerceSku> CommerceSkus { get; set; } = [];
}
