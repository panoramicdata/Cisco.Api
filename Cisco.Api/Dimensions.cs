using System.Runtime.Serialization;

namespace Cisco.Api
{
	[DataContract]
	public class Dimensions
	{
		[DataMember(Name = "dimensions_format")]
		public string Format { get; set; }

		[DataMember(Name = "dimensions_value")]
		public string Value { get; set; }
	}
}