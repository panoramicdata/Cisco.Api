using System;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Cisco.Api.Exceptions
{
	/// <summary>
	/// A Cisco Api exception
	/// </summary>
	[Serializable]
	public class CiscoApiException : Exception
	{
		public HttpResponseMessage Response { get; } = null!;

		/// <summary>
		/// Constructor
		/// </summary>
		public CiscoApiException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="response"></param>
		public CiscoApiException(HttpResponseMessage response)
			: base((response.IsSuccessStatusCode ? "An API issue occurred." : "API returned a non successful status code.") + " See \"Response\" property for more details.")
		{
			Response = response;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="response"></param>
		public CiscoApiException(HttpResponseMessage response, Exception innerException)
			: base((response.IsSuccessStatusCode ? "An API issue occurred." : "API returned a non successful status code.") + " See \"Response\" property for more details.", innerException)
		{
			Response = response;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public CiscoApiException(string message) : base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public CiscoApiException(string message, Exception innerException) : base(message, innerException)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		protected CiscoApiException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}