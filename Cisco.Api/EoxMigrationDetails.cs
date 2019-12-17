using System.Runtime.Serialization;

namespace Cisco.Api
{
	[DataContract]
	public class EoxMigrationDetails
	{
		[DataMember(Name= "PIDActiveFlag")]
		public string PidActiveFlag { get; set; }

		[DataMember(Name = "MigrationInformation")]
		public string Information { get; set; }

		[DataMember(Name = "MigrationOption")]
		public string Option { get; set; }

		[DataMember(Name = "MigrationProductId")]
		public string ProductId { get; set; }

		[DataMember(Name = "MigrationProductName")]
		public string ProductName { get; set; }

		[DataMember(Name = "MigrationStrategy")]
		public string Strategy { get; set; }

		[DataMember(Name = "MigrationProductInfoURL")]
		public string ProductInfoUrl { get; set; }
	}
}