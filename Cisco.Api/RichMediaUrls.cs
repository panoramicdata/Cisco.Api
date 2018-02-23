using System.Runtime.Serialization;

namespace Cisco.Api
{
	[DataContract]
	public class RichMediaUrls
	{
		[DataMember(Name = "large_image_url")]
		public string LargeImageUrl { get; set; }

		[DataMember(Name = "small_image_url")]
		public string SmallImageUrl { get; set; }
	}
}