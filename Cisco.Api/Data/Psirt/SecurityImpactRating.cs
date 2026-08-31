using System.Runtime.Serialization;

namespace Cisco.Api.Data.Psirt;

    /// <summary>
    /// Defines the supported security impact rating values.
    /// </summary>
    [DataContract]
    public enum SecurityImpactRating
    {
        /// <summary>
        /// Represents the critical value.
        /// </summary>
        [EnumMember(Value = "Critical")]
        Critical,

        /// <summary>
        /// Represents the high value.
        /// </summary>
        [EnumMember(Value = "High")]
        High,

        /// <summary>
        /// Represents the informational value.
        /// </summary>
        [EnumMember(Value = "Informational")]
        Informational,

        /// <summary>
        /// Represents the low value.
        /// </summary>
        [EnumMember(Value = "Low")]
        Low,

        /// <summary>
        /// Represents the medium value.
        /// </summary>
        [EnumMember(Value = "Medium")]
        Medium,

        /// <summary>
        /// Represents the na value.
        /// </summary>
        [EnumMember(Value = "NA")]
        Na
    };
