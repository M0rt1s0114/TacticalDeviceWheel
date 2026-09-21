namespace TacticalDeviceWheel.Core;

public sealed class SelectionMemory
{
	public int LastValidSelection { get; private set; } = -1;


	public int Update(double x, double y, int count, double deadZone)
	{
		if (LastValidSelection >= count)
		{
			LastValidSelection = -1;
		}
		int num = RadialMath.Select(x, y, count, deadZone);
		if (num >= 0)
		{
			LastValidSelection = num;
		}
		return LastValidSelection;
	}

	public void Reset()
	{
		LastValidSelection = -1;
	}
}
