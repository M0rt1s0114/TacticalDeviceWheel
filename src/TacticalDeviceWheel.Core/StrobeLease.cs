namespace TacticalDeviceWheel.Core;

public sealed class StrobeLease
{
	public StrobePlan Plan { get; }

	public NativeTarget Expected { get; private set; }

	public bool Restoring { get; private set; }

	public StrobeLease(StrobePlan plan)
	{
		Plan = plan;
		Expected = plan.Original;
	}

	public bool Owns(bool active, int mode)
	{
		if (active == Expected.IsActive)
		{
			return mode == Expected.Mode;
		}
		return false;
	}

	public void Stop()
	{
		Restoring = true;
	}

	public NativeTarget Desired(bool phaseOn)
	{
		if (!Restoring)
		{
			if (!phaseOn)
			{
				return Plan.Off;
			}
			return Plan.On;
		}
		return Plan.Original;
	}

	public void Accepted(NativeTarget target)
	{
		Expected = target;
	}
}
