using System;

namespace Cisco.Api.Exceptions;

/// <summary>
/// Exception thrown when there is an error with PX Cloud report operations
/// </summary>
[Serializable]
public class PxCloudReportException : CiscoApiException
{
	/// <summary>
	/// Constructor
	/// </summary>
	public PxCloudReportException()
	{
	}

	/// <summary>
	/// Constructor with message
	/// </summary>
	/// <param name="message">The error message</param>
	public PxCloudReportException(string message) : base(message)
	{
	}

	/// <summary>
	/// Constructor with message and inner exception
	/// </summary>
	/// <param name="message">The error message</param>
	/// <param name="innerException">The inner exception</param>
	public PxCloudReportException(string message, Exception innerException) : base(message, innerException)
	{
	}
}
