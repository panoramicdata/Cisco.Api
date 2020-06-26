using System.Runtime.Serialization;

namespace Cisco.Api.Data.ProductInfo
{
	/// <summary>
	/// Product information associated with a serial number
	/// </summary>
	[DataContract]
	public class ProductInformation
	{
		/// <summary>
		/// Product dimensions (dimension format and dimension value).
		/// </summary>
		[DataMember(Name = "dimensions")]
		public Dimensions Dimensions { get; set; } = null!;

		/// <summary>
		/// Form factor of the product if it exists.
		/// </summary>
		[DataMember(Name = "form_factor")]
		public string FormFactor { get; set; } = null!;

		/// <summary>
		/// Record number in a result set.
		/// </summary>
		[DataMember(Name = "id")]
		public string Id { get; set; } = null!;

		/// <summary>
		/// Base product ID for a given serial number.
		/// </summary>
		[DataMember(Name = "base_pid")]
		public string BasePid { get; set; } = null!;

		/// <summary>
		/// Orderable product ID for a given serial number.
		/// </summary>
		[DataMember(Name = "orderable_pid")]
		public string OrderablePid { get; set; } = null!;

		/// <summary>
		/// Represents the orderable status of a product. One of the following values:
		///	O = ORDERABLE
		///	N = NON_ORDERABLE
		///	EOX = End of Life
		/// </summary>
		[DataMember(Name = "orderable_status")]
		public string OrderableStatus { get; set; } = null!;

		/// <summary>
		/// Category of the product.
		/// </summary>
		[DataMember(Name = "product_category")]
		public string ProductCategory { get; set; } = null!;

		/// <summary>
		/// Title or description of the product.
		/// </summary>
		[DataMember(Name = "product_name")]
		public string ProductName { get; set; } = null!;

		/// <summary>
		/// Series to which the product belongs.
		/// </summary>
		[DataMember(Name = "product_series")]
		public string ProductSeries { get; set; } = null!;

		/// <summary>
		/// Subcategory of the product if it exists.
		/// </summary>
		[DataMember(Name = "product_subcategory")]
		public string ProductSubcategory { get; set; } = null!;

		/// <summary>
		/// 	URL of the Cisco.com support product model (or series) page.
		/// </summary>
		[DataMember(Name = "product_support_page")]
		public string ProductSupportPage { get; set; } = null!;

		/// <summary>
		/// 	Type of product. Note: For the 1.0 release, only hardware products are supported.
		/// </summary>
		[DataMember(Name = "product_type")]
		public string ProductType { get; set; } = null!;

		/// <summary>
		/// Product release date in the following format: YYYY-MM-DD.
		/// </summary>
		[DataMember(Name = "release_date")]
		public string ReleaseDate { get; set; } = null!;

		/// <summary>
		/// List of small and large image URLs for a given product if they exist. If multiple URLs exist, the URLs in the list will be separated by commas.
		/// </summary>
		[DataMember(Name = "rich_media_urls")]
		public RichMediaUrls RichMediaUrls { get; set; } = null!;

		/// <summary>
		/// Serial number submitted in the request.
		/// </summary>
		[DataMember(Name = "sr_no")]
		public string SerialNumber { get; set; } = null!;

		/// <summary>
		/// URL from which the .zip file of the Visio stencils for a given product can be downloaded if it exists.
		/// </summary>
		[DataMember(Name = "visio_stencil_url")]
		public string VisioStencilUrl { get; set; } = null!;

		/// <summary>
		/// Product weight with unit of measurement.
		/// </summary>
		[DataMember(Name = "weight")]
		public string Weight { get; set; } = null!;
	}
}