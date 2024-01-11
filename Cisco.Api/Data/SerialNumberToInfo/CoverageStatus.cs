using System.Runtime.Serialization;

namespace Cisco.Api.Data.SerialNumberToInfo;

/// <summary>
/// A coverage status
/// </summary>
[DataContract]
public class CoverageStatus
{
	/// <summary>
	/// The serial number
	/// </summary>
	[DataMember(Name = "sr_no")]
	public string SerialNumber { get; set; } = null!;

	/// <summary>
	/// Whether it is covered
	/// </summary>
	[DataMember(Name = "is_covered")]
	public string IsCovered { get; set; } = null!;

	/// <summary>
	/// The coverage end date
	/// </summary>
	[DataMember(Name = "coverage_end_date")]
	public string CoverageEndDate { get; set; } = null!;
}