using System;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Cisco.Api.Exceptions
{
	[Serializable]
	public class CiscoApiException : Exception
	{
		public HttpResponseMessage Response { get; }

		public CiscoApiException()
		{
		}

		public CiscoApiException(HttpResponseMessage response)
			: base((response.IsSuccessStatusCode ? "An API issue occurred." : "API returned a non successful status code.") + " See \"Response\" property for more details.")
			=> Response = response;

		public CiscoApiException(string message) : base(message)
		{
		}

		public CiscoApiException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected CiscoApiException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}