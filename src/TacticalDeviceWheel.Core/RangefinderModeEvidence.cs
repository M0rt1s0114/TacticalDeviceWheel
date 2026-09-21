namespace TacticalDeviceWheel.Core;

public static class RangefinderModeEvidence
{
	public static bool IsControlled(bool enabled, bool controllerWithinMode, bool canvasWithinMode, bool textWithinMode)
	{
		if (enabled)
		{
			return controllerWithinMode || canvasWithinMode || textWithinMode;
		}
		return false;
	}
}
