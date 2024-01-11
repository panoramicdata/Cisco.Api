using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SoftwareSuggestion
{
    /// <summary>
    /// A Software information page
    /// </summary>
    [DataContract]
    public class SoftwareSuggestionPage
    {
        /// <summary>
        /// Pagination response record
        /// </summary>
        [DataMember(Name = "paginationResponseRecord")]
        public PaginationResponseRecord PaginationResponseRecord { get; set; } = null!;

        /// <summary>
        /// The product list
        /// </summary>
        [DataMember(Name = "productList")]
        public List<SoftwareSuggestionProductList> Suggestions { get; set; } = null!;

		/// <summary>
		/// The error details response
		/// </summary>
		[DataMember(Name = "errorDetailsResponse")]
		public ErrorDetailsResponse? ErrorDetailsResponse { get; set; } = null!;

		/// <summary>
		/// The status
		/// </summary>
		[DataMember(Name = "status")]
		public string Status { get; set; } = string.Empty;
	}
}