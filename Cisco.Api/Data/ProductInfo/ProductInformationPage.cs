using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.ProductInfo
{
    /// <summary>
    /// A product information page
    /// </summary>
    [DataContract]
    public class ProductInformationPage
    {
        /// <summary>
        /// Pagination response record
        /// </summary>
        [DataMember(Name = "pagination_response_record")]
        public PaginationResponseRecord PaginationResponseRecord { get; set; } = null!;

        /// <summary>
        /// The product list
        /// </summary>
        [DataMember(Name = "product_list")]
        public List<ProductInformation> Products { get; set; } = null!;
    }
}