using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing;

/// <summary>
/// Represents the license item.
/// </summary>
[DataContract]
[DebuggerDisplay("{License}")]
public class LicenseItem
{
	/// <summary>
	/// Gets or sets the is portable flag.
	/// </summary>
	[DataMember(Name = "isPortable")]
	public bool IsPortable { get; set; }

	/// <summary>
	/// Gets or sets the license substitutions.
	/// </summary>
	[DataMember(Name = "licenseSubstitutions")]
	public List<LicenseSubstitution> LicenseSubstitutions { get; set; } = new();

	/// <summary>
	/// Gets or sets the quantity.
	/// </summary>
	[DataMember(Name = "quantity")]
	public int Quantity { get; set; }

	/// <summary>
	/// Gets or sets the aha apps flag.
	/// </summary>
	[DataMember(Name = "ahaApps")]
	public bool AhaApps { get; set; }

	/// <summary>
	/// Gets or sets the available.
	/// </summary>
	[DataMember(Name = "available")]
	public int Available { get; set; }

	/// <summary>
	/// Gets or sets the license.
	/// </summary>
	[DataMember(Name = "license")]
	public string License { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the license details.
	/// </summary>
	[DataMember(Name = "licenseDetails")]
	public List<LicenseDetail> LicenseDetails { get; set; } = new();

	/// <summary>
	/// Gets or sets the billing type.
	/// </summary>
	[DataMember(Name = "billingType")]
	public string BillingType { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the pending quantity.
	/// </summary>
	[DataMember(Name = "pendingQuantity")]
	public int PendingQuantity { get; set; }

	/// <summary>
	/// Gets or sets the reserved.
	/// </summary>
	[DataMember(Name = "reserved")]
	public int Reserved { get; set; }

	/// <summary>
	/// Gets or sets the in use.
	/// </summary>
	[DataMember(Name = "inUse")]
	public int InUse { get; set; }

	/// <summary>
	/// Gets or sets the virtual account.
	/// </summary>
	[DataMember(Name = "virtualAccount")]
	public string VirtualAccount { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the status.
	/// </summary>
	[DataMember(Name = "status")]
	public string Status { get; set; } = string.Empty;
}
