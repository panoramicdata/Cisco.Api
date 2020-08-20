using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cisco.Api.Data.Psirt
{

	[DataContract]
	public partial class Advisory
	{
		[DataMember(Name = "advisoryId")]
		public string AdvisoryId { get; set; } = null!;

		[DataMember(Name = "advisoryTitle")]
		public string AdvisoryTitle { get; set; } = null!;

		[DataMember(Name = "bugIDs")]
		public List<string> BugIds { get; set; } = null!;

		[DataMember(Name = "ipsSignatures")]
		public List<object> IpsSignatures { get; set; } = null!;

		[DataMember(Name = "cves")]
		public List<string> Cves { get; set; } = null!;

		[DataMember(Name = "cvrfUrl")]
		public string CvrfUrl { get; set; } = null!;

		[DataMember(Name = "cvssBaseScore")]
		public string CvssBaseScore { get; set; } = null!;

		[DataMember(Name = "cwe")]
		public List<string> Cwe { get; set; } = null!;

		[DataMember(Name = "firstPublished")]
		public DateTimeOffset FirstPublished { get; set; }

		[DataMember(Name = "lastUpdated")]
		public DateTimeOffset LastUpdated { get; set; }

		[DataMember(Name = "productNames")]
		public List<string> ProductNames { get; set; } = null!;

		[DataMember(Name = "publicationUrl")]
		public string PublicationUrl { get; set; } = null!;

		[DataMember(Name = "sir")]
		public SecurityImpactRating SecurityImpactRating { get; set; }

		[DataMember(Name = "summary")]
		public string Summary { get; set; } = null!;
	}
}
