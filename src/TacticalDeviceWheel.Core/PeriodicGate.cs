namespace TacticalDeviceWheel.Core;

public sealed class PeriodicGate
{
	private double last = double.NegativeInfinity;

	public bool Take(double now, double rate)
	{
		if (double.IsNaN(now) || double.IsInfinity(now) || double.IsNaN(rate) || double.IsInfinity(rate) || rate <= 0.0)
		{
			return false;
		}
		if (now - last < 1.0 / rate)
		{
			return false;
		}
		last = now;
		return true;
	}

	public void Reset()
	{
		last = double.NegativeInfinity;
	}
}
