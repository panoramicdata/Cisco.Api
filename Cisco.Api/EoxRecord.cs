namespace Cisco.Api
{
	public class EoxRecord
	{
		public string EolProductId { get; set; }
		public string ProductIdDescription { get; set; }
		public string ProductBulletinNumber { get; set; }
		public string LinkToProductBulletinUrl { get; set; }
		public CiscoDate EoxExternalAnnouncementCiscoDate { get; set; }
		public CiscoDate EndOfSaleCiscoDate { get; set; }
		public CiscoDate EndOfSwMaintenanceReleases { get; set; }
		public CiscoDate EndOfSecurityVulSupportCiscoDate { get; set; }
		public CiscoDate EndOfRoutineFailureAnalysisCiscoDate { get; set; }
		public CiscoDate EndOfServiceContractRenewal { get; set; }
		public CiscoDate LastCiscoDateOfSupport { get; set; }
		public CiscoDate EndOfSvcAttachCiscoDate { get; set; }
		public CiscoDate UpdatedTimeStamp { get; set; }
		public EoxMigrationDetails EoxMigrationDetails { get; set; }
		public string EoxInputType { get; set; }
		public string EoxInputValue { get; set; }
	}
}