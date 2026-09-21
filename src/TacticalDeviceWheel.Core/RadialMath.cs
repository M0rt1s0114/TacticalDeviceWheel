using System;

namespace TacticalDeviceWheel.Core;

public static class RadialMath
{
	public static int Select(double x, double y, int count, double deadZone)
	{
		if (count <= 0 || x * x + y * y < deadZone * deadZone)
		{
			return -1;
		}
		return (int)Math.Floor(((90.0 - Math.Atan2(y, x) * 180.0 / Math.PI + 360.0) % 360.0 + 180.0 / (double)count) / (360.0 / (double)count)) % count;
	}
}
