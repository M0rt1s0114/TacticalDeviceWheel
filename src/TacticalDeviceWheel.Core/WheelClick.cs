namespace TacticalDeviceWheel.Core;

public static class WheelClick
{
	public static WheelClickAction Decide(bool tHeld, bool leftDown, bool rightDown, bool hasSelection, bool closeOnTRelease = true)
	{
		if ((!tHeld && closeOnTRelease) || rightDown)
		{
			return WheelClickAction.Cancel;
		}
		if (!(leftDown && hasSelection))
		{
			return WheelClickAction.None;
		}
		return WheelClickAction.Confirm;
	}
}
