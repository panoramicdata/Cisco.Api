using System.Runtime.Serialization;

namespace Cisco.Api
{
	[DataContract]
	public class AccessTokenResponse
	{
		[DataMember(Name= "access_token")]
		public string AccessToken { get; set; }

		[DataMember(Name = "token_type")]
		public string TokenType { get; set; }

		[DataMember(Name = "expires_in")]
		public int ExpiresIn { get; set; }

		[DataMember(Name = "error_description")]
		public string ErrorDescription { get; set; }

		[DataMember(Name = "error")]
		public string Error { get; set; }
	}
}