using System.Runtime.Serialization;

namespace Cisco.Api.Data.ProductInfo;

    /// <summary>
    /// An API error
    /// </summary>
    [DataContract]
    public class ApiError
    {
        /// <summary>
        /// The error code
        /// </summary>
        [DataMember(Name = "ErrorCode")]
        public string ErrorCode { get; set; } = null!;

        /// <summary>
        /// The error description
        /// </summary>
        [DataMember(Name = "ErrorDescription")]
        public string ErrorDescription { get; set; } = null!;

        /// <summary>
        /// The suggested action
        /// </summary>
        [DataMember(Name = "SuggestedAction")]
        public string SuggestedAction { get; set; } = null!;
    }