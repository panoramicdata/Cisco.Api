using System.Runtime.Serialization;

namespace Cisco.Api.Data.ProductInfo
{
	/// <summary>
	/// List of small and large image URLs for a given product if they exist. If multiple URLs exist, the URLs in the list will be separated by commas
	/// </summary>
	[DataContract]
	public class RichMediaUrls
	{
		/// <summary>
		/// List of small image URLs for a given product if they exist. If multiple URLs exist, the URLs in the list will be separated by commas
		/// </summary>
		[DataMember(Name = "large_image_url")]
		public string LargeImageUrl { get; set; } = null!;

		/// <summary>
		/// List of large image URLs for a given product if they exist. If multiple URLs exist, the URLs in the list will be separated by commas
		/// </summary>
		[DataMember(Name = "small_image_url")]
		public string SmallImageUrl { get; set; } = null!;
	}
}