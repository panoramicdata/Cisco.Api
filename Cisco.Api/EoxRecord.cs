using System.Runtime.Serialization;

namespace Cisco.Api
{
	[DataContract]
	public class EoxRecord
	{
		[DataMember(Name = "EolProductID")]
		public string EolProductId { get; set; }

		[DataMember(Name = "ProductIDDescription")]
		public string ProductIdDescription { get; set; }

		[DataMember(Name = "ProductBulletinNumber")]
		public string ProductBulletinNumber { get; set; }

		[DataMember(Name = "LinkToProductBulletinURL")]
		public string LinkToProductBulletinUrl { get; set; }

		[DataMember(Name = "EoxExternalAnnouncementDate")]
		public CiscoDate ExternalAnnouncementDate { get; set; }

		[DataMember(Name = "EndOfSaleDate")]
		public CiscoDate EndOfSaleDate { get; set; }

		[DataMember(Name = "EndOfSWMaintenanceReleases")]
		public CiscoDate EndOfSoftwareMaintenanceReleases { get; set; }

		[DataMember(Name = "EndOfSecurityVulSupportDate")]
		public CiscoDate EndOfSecurityVulnerabilitySupportDate { get; set; }

		[DataMember(Name = "EndOfRoutineFailureAnalysisDate")]
		public CiscoDate EndOfRoutineFailureAnalysisDate { get; set; }

		[DataMember(Name = "EndOfServiceContractRenewal")]
		public CiscoDate EndOfServiceContractRenewalDate { get; set; }

		[DataMember(Name = "LastDateOfSupport")]
		public CiscoDate LastSupportDate { get; set; }

		[DataMember(Name = "EndOfSvcAttachDate")]
		public CiscoDate EndOfSvcAttachDate { get; set; }

		[DataMember(Name = "UpdatedTimestamp")]
		public CiscoDate UpdatedDate { get; set; }

		[DataMember(Name = "EOXMigrationDetails")]
		public EoxMigrationDetails EoxMigrationDetails { get; set; }

		[DataMember(Name = "EoxInputType")]
		public string InputType { get; set; }

		[DataMember(Name = "EoxInputValue")]
		public string InputValue { get; set; }
	}
}