namespace Cisco.Api;

/// <summary>
/// Represents the API access.
/// </summary>
public class ApiAccess
{
	/// <summary>
	/// Gets or sets the enterprise agreement flag.
	/// </summary>
	public bool EnterpriseAgreement { get; internal set; }
	/// <summary>
	/// Gets or sets the EoX flag.
	/// </summary>
	public bool Eox { get; internal set; }
	/// <summary>
	/// Gets or sets the hello flag.
	/// </summary>
	public bool Hello { get; internal set; }
	/// <summary>
	/// Gets or sets the product info flag.
	/// </summary>
	public bool ProductInfo { get; internal set; }
	/// <summary>
	/// Gets or sets the PSIRT flag.
	/// </summary>
	public bool Psirt { get; internal set; }
	/// <summary>
	/// Gets or sets the PSS flag.
	/// </summary>
	public bool Pss { get; internal set; }
	/// <summary>
	/// Gets or sets the serial number to info flag.
	/// </summary>
	public bool SerialNumberToInfo { get; internal set; }
	/// <summary>
	/// Gets or sets the smart accounts and licensing flag.
	/// </summary>
	public bool SmartAccountsAndLicensing { get; internal set; }
	/// <summary>
	/// Gets or sets the software suggestion flag.
	/// </summary>
	public bool SoftwareSuggestion { get; internal set; }
	/// <summary>
	/// Gets or sets the umbrella flag.
	/// </summary>
	public bool Umbrella { get; internal set; }

	/// <summary>
	/// Represents the any value.
	/// </summary>
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