using System.Collections.Generic;

namespace TacticalDeviceWheel.Core;

public sealed class NativeModeDefinition
{
	public int Mode { get; set; }

	public List<string> Capabilities { get; set; }
}
