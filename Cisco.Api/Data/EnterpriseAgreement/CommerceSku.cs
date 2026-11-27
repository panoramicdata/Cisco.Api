using System.Runtime.Serialization;

namespace Cisco.Api.Data.EnterpriseAgreement.Responses;

/// <summary>
/// Represents the commerce SKU.
/// </summary>
[DataContract]
public class CommerceSku : SuiteEntitlements
{
	/// <summary>
	/// Gets or sets the EOL flag.
	/// </summary>
	[DataMember(Name = "eol")]
	public bool Eol { get; set; }

	/// <summary>
	/// Gets or sets the suite commerce SKU.
	/// </summary>
	[DataMember(Name = "commerceSku")]
	public string SuiteCommerceSku { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the suite commerce SKU description.
	/// </summary>
	[DataMember(Name = "commerceSkuDescription")]
	public string SuiteCommerceSkuDescription { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the EOL message.
	/// </summary>
	[DataMember(Name = "eolMessage")]
	public string EolMessage { get; set; } = string.Empty;

}
