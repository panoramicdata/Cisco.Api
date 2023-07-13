using System.Runtime.Serialization;

namespace Cisco.Api.Data.ProductInfo
{
    /// <summary>
    /// An error response
    /// </summary>
    [DataContract]
    public class ErrorResponse
    {
        /// <summary>
        /// The API Error
        /// </summary>
        [DataMember(Name = "APIError")]
        public ApiError ApiError { get; set; } = null!;
    }
}