using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing;

[DataContract]
[DebuggerDisplay("{License}")]
public class LicenseItem
{
	[DataMember(Name = "isPortable")]
	public bool IsPortable { get; set; }

	[DataMember(Name = "licenseSubstitutions")]
	public List<string> LicenseSubstitutions { get; set; } = new();

	[DataMember(Name = "quantity")]
	public int Quantity { get; set; }

	[DataMember(Name = "ahaApps")]
	public bool AhaApps { get; set; }

	[DataMember(Name = "available")]
	public int Available { get; set; }

	[DataMember(Name = "license")]
	public string License { get; set; } = string.Empty;

	[DataMember(Name = "licenseDetails")]
	public List<LicenseDetail> LicenseDetails { get; set; } = new();

	[DataMember(Name = "billingType")]
	public string BillingType { get; set; } = string.Empty;

	[DataMember(Name = "pendingQuantity")]
	public int PendingQuantity { get; set; }

	[DataMember(Name = "reserved")]
	public int Reserved { get; set; }

	[DataMember(Name = "inUse")]
	public int InUse { get; set; }

	[DataMember(Name = "virtualAccount")]
	public string VirtualAccount { get; set; } = string.Empty;

	[DataMember(Name = "status")]
	public string Status { get; set; } = string.Empty;
}
