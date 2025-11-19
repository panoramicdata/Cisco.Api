using System;

namespace Cisco.Api.Exceptions;

/// <summary>
/// Exception thrown when there is an error with PSS configuration operations
/// </summary>
[Serializable]
public class PssConfigException : CiscoApiException
{
	/// <summary>
	/// Constructor
	/// </summary>
	public PssConfigException()
	{
	}

	/// <summary>
	/// Constructor with message
	/// </summary>
	/// <param name="message">The error message</param>
	public PssConfigException(string message) : base(message)
	{
	}

	/// <summary>
	/// Constructor with message and inner exception
	/// </summary>
	/// <param name="message">The error message</param>
	/// <param name="innerException">The inner exception</param>
	public PssConfigException(string message, Exception innerException) : base(message, innerException)
	{
	}
}
