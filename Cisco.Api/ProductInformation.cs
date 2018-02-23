using System.Runtime.Serialization;

namespace Cisco.Api
{
	[DataContract]
	public class ProductInformation
	{
		[DataMember(Name = "id")]
		public string Id { get; set; }

		[DataMember(Name = "sr_no")]
		public string SrNo { get; set; }

		[DataMember(Name = "base_pid")]
		public string BasePid { get; set; }

		[DataMember(Name = "orderable_pid")]
		public string OrderablePid { get; set; }

		[DataMember(Name = "product_name")]
		public string ProductName { get; set; }

		[DataMember(Name = "product_type")]
		public string ProductType { get; set; }

		[DataMember(Name = "product_series")]
		public string ProductSeries { get; set; }

		[DataMember(Name = "product_category")]
		public string ProductCategory { get; set; }

		[DataMember(Name = "product_subcategory")]
		public string ProductSubcategory { get; set; }

		[DataMember(Name = "release_date")]
		public string ReleaseDate { get; set; }

		[DataMember(Name = "orderable_status")]
		public string OrderableStatus { get; set; }

		[DataMember(Name = "dimensions")]
		public Dimensions Dimensions { get; set; }

		[DataMember(Name = "weight")]
		public string Weight { get; set; }

		[DataMember(Name = "form_factor")]
		public string FormFactor { get; set; }

		[DataMember(Name = "product_support_page")]
		public string ProductSupportPage { get; set; }

		[DataMember(Name = "visio_stencil_url")]
		public string VisioStencilUrl { get; set; }

		[DataMember(Name = "rich_media_urls")]
		public RichMediaUrls RichMediaUrls { get; set; }
	}
}