using System.Collections.Generic;
using System.Linq;

namespace TacticalDeviceWheel.Core;

public sealed class StrobePlan
{
	public NativeTarget Original { get; private set; }

	public NativeTarget On { get; private set; }

	public NativeTarget Off { get; private set; }

	public static bool TryCreate(NativeModeMap map, bool active, int mode, ulong whiteLight, out StrobePlan plan)
	{
		plan = null;
		if (map == null || !map.Modes.ContainsKey(mode) || whiteLight == 0L || (whiteLight & (whiteLight - 1)) != 0L || !map.TryCurrent(active, mode, out var mask))
		{
			return false;
		}
		ulong num = mask & ~whiteLight;
		if (!Find(map, num | whiteLight, mode, out var target) || !Find(map, num, mode, out var target2))
		{
			return false;
		}
		plan = new StrobePlan
		{
			Original = new NativeTarget(active, mode, mask, 0uL, 0uL),
			On = target,
			Off = target2
		};
		return true;
	}

	private static bool Find(NativeModeMap map, ulong mask, int preferredMode, out NativeTarget target)
	{
		target = default(NativeTarget);
		if (mask == 0L)
		{
			if (map.Modes.Count == 0)
			{
				return false;
			}
			target = new NativeTarget(active: false, map.Modes.ContainsKey(preferredMode) ? preferredMode : map.Modes.Keys.First(), 0uL, 0uL, 0uL);
			return true;
		}
		if (map.Modes.TryGetValue(preferredMode, out var value) && value == mask)
		{
			target = new NativeTarget(active: true, preferredMode, mask, 0uL, 0uL);
			return true;
		}
		foreach (KeyValuePair<int, ulong> mode in map.Modes)
		{
			if (mode.Value == mask)
			{
				target = new NativeTarget(active: true, mode.Key, mask, 0uL, 0uL);
				return true;
			}
		}
		return false;
	}
}
