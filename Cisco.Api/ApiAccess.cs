namespace Cisco.Api;

public class ApiAccess
{
	public bool EnterpriseAgreement { get; internal set; }
	public bool Eox { get; internal set; }
	public bool Hello { get; internal set; }
	public bool ProductInfo { get; internal set; }
	public bool Psirt { get; internal set; }
	public bool Pss { get; internal set; }
	public bool SerialNumberToInfo { get; internal set; }
	public bool SmartAccountsAndLicensing { get; internal set; }
	public bool SoftwareSuggestion { get; internal set; }
	public bool Umbrella { get; internal set; }

	public bool Any => EnterpriseAgreement
		|| Eox
		|| Hello
		|| ProductInfo
		|| Psirt
		|| Pss 
		|| SerialNumberToInfo 
		|| SmartAccountsAndLicensing 
		|| SoftwareSuggestion 
		|| Umbrella;
}