using System;

namespace TacticalDeviceWheel.Compatibility;

internal sealed class DeviceVisuals
{
	public string InstanceId;

	public string TemplateId;

	public string Error;

	public VisualMode[] Modes = Array.Empty<VisualMode>();
}
