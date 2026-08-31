using System;

namespace Cisco.Api.Data.Pss;

/// <summary>
/// Represents the device config response.
/// </summary>
public class DeviceConfigResponse
{
	/// <summary>
	/// Gets or sets the startup config.
	/// </summary>
	public string? StartupConfig { get; set; }

	/// <summary>
	/// Gets or sets the startup config date.
	/// </summary>
	public DateTime? StartupConfigDate { get; set; }

	/// <summary>
	/// Gets or sets the running config.
	/// </summary>
	public string? RunningConfig { get; set; }

	/// <summary>
	/// Gets or sets the running config date.
	/// </summary>
	public DateTime? RunningConfigDate { get; set; }
}