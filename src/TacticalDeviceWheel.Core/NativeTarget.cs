namespace TacticalDeviceWheel.Core;

public readonly struct NativeTarget
{
	public readonly bool IsActive;

	public readonly int Mode;

	public readonly ulong CapabilityMask;

	public readonly ulong AutomaticallyEnabledMask;

	public readonly ulong AutomaticallyDisabledMask;

	public NativeTarget(bool active, int mode, ulong mask, ulong automaticallyEnabled = 0uL, ulong automaticallyDisabled = 0uL)
	{
		IsActive = active;
		Mode = mode;
		CapabilityMask = mask;
		AutomaticallyEnabledMask = automaticallyEnabled;
		AutomaticallyDisabledMask = automaticallyDisabled;
	}
}
