using System.Runtime.Serialization;

namespace Cisco.Api.Security
{
	/// <summary>
	/// An access token response
	/// </summary>
	[DataContract]
	public class AccessTokenResponse
	{
		/// <summary>
		/// The access token
		/// </summary>
		[DataMember(Name = "access_token")]
		public string? AccessToken { get; set; }

		/// <summary>
		/// The token type
		/// </summary>
		[DataMember(Name = "token_type")]
		public string? TokenType { get; set; }

		/// <summary>
		/// The number of ? to expiry
		/// </summary>
		[DataMember(Name = "expires_in")]
		public int? ExpiresIn { get; set; }

		/// <summary>
		/// The error description
		/// </summary>
		[DataMember(Name = "error_description")]
		public string? ErrorDescription { get; set; }

		/// <summary>
		/// The error
		/// </summary>
		[DataMember(Name = "error")]
		public string? Error { get; set; }
	}
}