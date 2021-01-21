using System.Runtime.Serialization;

namespace Cisco.Api.Data.Eox
{
	/// <summary>
	/// EOX migration details
	/// </summary>
	[DataContract]
	public class EoxMigrationDetails
	{
		/// <summary>
		/// The PID activity flag
		/// </summary>
		[DataMember(Name = "PIDActiveFlag")]
		public string PidActiveFlag { get; set; } = null!;

		/// <summary>
		/// The information
		/// </summary>
		[DataMember(Name = "MigrationInformation")]
		public string Information { get; set; } = null!;

		/// <summary>
		/// The option
		/// </summary>
		[DataMember(Name = "MigrationOption")]
		public string Option { get; set; } = null!;

		/// <summary>
		/// The product ID
		/// </summary>
		[DataMember(Name = "MigrationProductId")]
		public string ProductId { get; set; } = null!;

		/// <summary>
		/// The product name
		/// </summary>
		[DataMember(Name = "MigrationProductName")]
		public string ProductName { get; set; } = null!;

		/// <summary>
		/// The strategy
		/// </summary>
		[DataMember(Name = "MigrationStrategy")]
		public string Strategy { get; set; } = null!;

		/// <summary>
		/// The product info url
		/// </summary>
		[DataMember(Name = "MigrationProductInfoURL")]
		public string ProductInfoUrl { get; set; } = null!;
	}
}