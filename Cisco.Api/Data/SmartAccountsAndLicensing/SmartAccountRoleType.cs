using System.Runtime.Serialization;

namespace Cisco.Api.Data.SmartAccountsAndLicensing;

public enum SmartAccountRoleType
{
	[DataMember(Name = "Smart Account Administrator")]
	AccountAdministrator,

	[DataMember(Name = "Smart Account User")]
	AccountUser,

	[DataMember(Name = "Smart Account Viewer")]
	AccountViewer,

	//

	[DataMember(Name = "Virtual Account Administrator")]
	VirtualAccountAdministrator,

	[DataMember(Name = "Virtual Account User")]
	VirtualAccountUser,

	[DataMember(Name = "Virtual Account Viewer")]
	VirtualAccountViewer,
}