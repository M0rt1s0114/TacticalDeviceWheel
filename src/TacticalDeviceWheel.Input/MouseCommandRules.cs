using EFT.InputSystem;

namespace TacticalDeviceWheel.Input;

internal static class MouseCommandRules
{
	public static bool ShouldBlock(ECommand command, bool wheelOpen, bool leftOwned, bool rightOwned)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected I4, but got Unknown
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		switch ((int)command - 1)
		{
		default:
			if ((int)command == 58)
			{
				return rightOwned;
			}
			return false;
		case 0:
			return wheelOpen || leftOwned;
		case 1:
			return leftOwned;
		case 2:
			return wheelOpen || rightOwned;
		}
	}
}
