using System;

namespace Cisco.Api.Data.Pss;

public class DeviceConfigResponse
{
	public string? StartupConfig { get; set; }

	public DateTime? StartupConfigDate { get; set; }

	public string? RunningConfig { get; set; }

	public DateTime? RunningConfigDate { get; set; }


}