using System.Collections.Generic;

namespace TacticalDeviceWheel.Core;

public sealed class DatabaseFile
{
	public int SchemaVersion { get; set; }

	public string SptVersion { get; set; }

	public Dictionary<string, DeviceDefinition> Devices { get; set; }
}
