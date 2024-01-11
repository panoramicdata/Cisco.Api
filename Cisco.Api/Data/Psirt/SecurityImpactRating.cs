using System.Runtime.Serialization;

namespace Cisco.Api.Data.Psirt;

    [DataContract]
    public enum SecurityImpactRating
    {
        [EnumMember(Value = "Critical")]
        Critical,

        [EnumMember(Value = "High")]
        High,

        [EnumMember(Value = "Informational")]
        Informational,

        [EnumMember(Value = "Low")]
        Low,

        [EnumMember(Value = "Medium")]
        Medium,

        [EnumMember(Value = "NA")]
        Na
    };
