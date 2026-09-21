using System.Collections.Generic;

namespace TacticalDeviceWheel.Core;

public sealed class DeviceDefinition
{
	public string Name { get; set; }

	public int ExpectedModes { get; set; }

	public string Evidence { get; set; }

	public string ModeResolver { get; set; }

	public string LaserSpectrum { get; set; }

	public bool IgnoreStatusIndicators { get; set; }

	public List<string> RequiredWhileActive { get; set; }

	public List<List<string>> ExclusiveGroups { get; set; }

	public List<CapabilityDefinition> Capabilities { get; set; }

	public List<NativeModeDefinition> Modes { get; set; }
}
