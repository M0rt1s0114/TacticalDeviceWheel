using System;

namespace TacticalDeviceWheel.Compatibility;

internal sealed class VisualSnapshot
{
	public string Error;

	public DeviceVisuals[] Devices = Array.Empty<DeviceVisuals>();
}
