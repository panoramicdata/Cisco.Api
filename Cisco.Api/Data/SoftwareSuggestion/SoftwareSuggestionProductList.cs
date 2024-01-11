using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SoftwareSuggestion
{
    /// <summary>
    /// Software suggestion information associated with a product ID
    /// </summary>
    [DataContract]
    public class SoftwareSuggestionProductList
	{
		/// <summary>
		/// Product
		/// </summary>
		[DataMember(Name = "product")]
		public SoftwareSuggestionProduct? Product { get; set; }

		/// <summary>
		/// ID
		/// </summary>
		[DataMember(Name = "id")]
		public int Id { get; set; }

		/// <summary>
		/// Record number in a result set.
		/// </summary>
		[DataMember(Name = "suggestions")]
		public List<SoftwareSuggestion> Suggestions { get; set; } = [];
	}
}