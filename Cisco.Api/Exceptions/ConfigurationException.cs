using System;

namespace Cisco.Api.Exceptions;

/// <summary>
/// Represents the configuration exception.
/// </summary>
[Serializable]
public class ConfigurationException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ConfigurationException"/> class.
	/// </summary>
	public ConfigurationException()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ConfigurationException"/> class.
	/// </summary>
	public ConfigurationException(string message) : base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ConfigurationException"/> class.
	/// </summary>
	public ConfigurationException(string message, Exception innerException) : base(message, innerException)
	{
	}
}