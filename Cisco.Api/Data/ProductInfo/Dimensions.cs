using System.Runtime.Serialization;

namespace Cisco.Api.Data.ProductInfo
{
	/// <summary>
	/// 	Product dimensions (dimension format and dimension value).
	/// </summary>
	[DataContract]
	public class Dimensions
	{
		/// <summary>
		/// Format
		/// </summary>
		[DataMember(Name = "dimensions_format")]
		public string Format { get; set; } = null!;

		/// <summary>
		/// Value
		/// </summary>
		[DataMember(Name = "dimensions_value")]
		public string Value { get; set; } = null!;
	}
}