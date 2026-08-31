using System.Diagnostics;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing;

/// <summary>
/// Represents the smart account with ids.
/// </summary>
[DataContract]
[DebuggerDisplay("{Name}")]
public class SmartAccountWithIds : SmartAccount
{
	/// <summary>
	/// Gets or sets the ID.
	/// </summary>
	[DataMember(Name = "id")]
	public required int Id { get; set; }
}
