using System.Runtime.Serialization;

namespace Cisco.Api.Data.Eox
{
	/// <summary>
	/// A Cisco date
	/// </summary>
	[DataContract]
	public class CiscoDate
	{
		/// <summary>
		/// The value
		/// </summary>
		[DataMember(Name = "value")]
		public string Value { get; set; } = null!;

		/// <summary>
		/// The date format
		/// </summary>
		[DataMember(Name = "dateFormat")]
		public string DateFormat { get; set; } = null!;
	}
}