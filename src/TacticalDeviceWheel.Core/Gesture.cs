using System;

namespace TacticalDeviceWheel.Core;

public sealed class Gesture
{
	private double started;

	private double threshold;

	public bool Pending { get; private set; }

	public bool Opened { get; private set; }

	public bool Active
	{
		get
		{
			if (!Pending)
			{
				return Opened;
			}
			return true;
		}
	}

	public void Begin(double now, double holdSeconds)
	{
		if (!Active)
		{
			started = now;
			threshold = Math.Max(0.05, holdSeconds);
			Pending = true;
		}
	}

	public GestureResult Step(double now, bool held, bool closeOnTRelease = true)
	{
		if (!Active)
		{
			return GestureResult.None;
		}
		if (Opened && !held && !closeOnTRelease)
		{
			return GestureResult.None;
		}
		if (!held)
		{
			int result = (Opened ? 3 : ((now - started < threshold) ? 2 : 4));
			Reset();
			return (GestureResult)result;
		}
		if (Pending && now - started >= threshold)
		{
			Pending = false;
			Opened = true;
			return GestureResult.Open;
		}
		return GestureResult.None;
	}

	public void Reset()
	{
		Pending = false;
		Opened = false;
	}
}
