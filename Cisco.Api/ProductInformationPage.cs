using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api
{
	[DataContract]
	public class ProductInformationPage
	{
		[DataMember(Name= "pagination_response_record")]
		public PaginationResponseRecord PaginationResponseRecord { get; set; }

		[DataMember(Name = "product_list")]
		public List<ProductInformation> Products { get; set; }
	}
}