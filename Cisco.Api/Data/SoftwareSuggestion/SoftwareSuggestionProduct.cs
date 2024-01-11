using System.Runtime.Serialization;

namespace Cisco.Api.Data.SoftwareSuggestion;

[DataContract]
public class SoftwareSuggestionProduct
{
	/// <summary>
	/// The base ID
	/// </summary>
	[DataMember(Name = "basePID")]
	public string BasePid{ get; set; } = string.Empty;

	/// <summary>
	/// The metadata framework ID
	/// </summary>
	[DataMember(Name = "mdfId")]
	public string MdfId { get; set; } = string.Empty;

	/// <summary>
	/// The product name
	/// </summary>
	[DataMember(Name = "productName")]
	public string ProductName { get; set; } = string.Empty;

	/// <summary>
	/// The software type
	/// </summary>
	[DataMember(Name = "softwareType")]
	public string Software { get; set; } = string.Empty;
}