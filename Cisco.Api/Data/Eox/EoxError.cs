using System.Runtime.Serialization;

namespace Cisco.Api.Data.Eox
{
	/// <summary>
	/// An EOX Error
	/// </summary>
	[DataContract]
	public class EoxError
	{
		/// <summary>
		/// Identification number associated with the error message.
		/// </summary>
		[DataMember(Name = "ErrorID")]
		public string ErrorID { get; set; } = null!;

		/// <summary>
		/// Description of the error message.
		/// </summary>
		[DataMember(Name = "ErrorDescription")]
		public string ErrorDescription { get; set; } = null!;

		/// <summary>
		/// Describes the type of error returned. This element is returned only if the requested serial number or software string can be mapped to a product ID or product bulletin, and the product ID or product bulletin does not correspond to end-of-life data. Possible values include:
		/// - PRODUCT_ID
		/// - PRODUCT_BULLETIN
		/// </summary>
		[DataMember(Name = "ErrorDataType")]
		public string ErrorDataType { get; set; } = null!;

		/// <summary>
		/// Describes the mapped product ID or product bulletin identified by the ErrorDataType. This element is returned only with the ErrorDataType parameter.
		/// </summary>
		[DataMember(Name = "ErrorDataValue")]
		public string ErrorDataValue { get; set; } = null!;
	}
}