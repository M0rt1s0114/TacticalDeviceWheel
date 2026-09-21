using System.Collections.Generic;

namespace TacticalDeviceWheel.Core;

public sealed class NativeLightSummary
{
	public int WhiteLights;

	public int InfraredLights;

	public readonly List<string> Unsupported = new List<string>();

	public readonly List<string> IgnoredIndicators = new List<string>();
}
