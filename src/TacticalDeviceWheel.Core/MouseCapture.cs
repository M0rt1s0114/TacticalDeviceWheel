namespace TacticalDeviceWheel.Core;

public sealed class MouseCapture
{
	private int releaseFrame = -100;

	public bool Captured { get; private set; }

	public void Capture()
	{
		Captured = true;
	}

	public void Observe(bool held, bool focused, int frame)
	{
		if (Captured && focused && !held)
		{
			Captured = false;
			releaseFrame = frame;
		}
	}

	public bool Blocks(int frame)
	{
		if (!Captured)
		{
			return releaseFrame == frame;
		}
		return true;
	}

	public void Reset()
	{
		Captured = false;
		releaseFrame = -100;
	}
}
