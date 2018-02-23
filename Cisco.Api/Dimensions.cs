using System.Runtime.Serialization;

namespace Cisco.Api
{
	[DataContract]
	public class Dimensions
	{
		[DataMember(Name = "dimensions_format")]
		public string DimensionsFormat { get; set; }

		[DataMember(Name = "dimensions_value")]
		public string DimensionsValue { get; set; }
	}
}