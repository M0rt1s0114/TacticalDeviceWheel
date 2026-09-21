using System;
using System.Collections.Generic;
using System.Linq;

namespace TacticalDeviceWheel.Core;

public sealed class NativeModeMap
{
	public IReadOnlyDictionary<int, ulong> Modes { get; }

	public ulong RequiredWhileActiveMask { get; }

	public IReadOnlyList<ulong> ExclusiveGroups { get; }

	public NativeModeMap(IDictionary<int, ulong> modes, ulong requiredWhileActive = 0uL, IEnumerable<ulong> exclusiveGroups = null)
	{
		Modes = new SortedDictionary<int, ulong>(modes);
		RequiredWhileActiveMask = requiredWhileActive;
		ExclusiveGroups = (exclusiveGroups ?? Array.Empty<ulong>()).ToArray();
	}

	public bool TryCurrent(bool active, int mode, out ulong mask)
	{
		mask = 0uL;
		if (active)
		{
			return Modes.TryGetValue(mode, out mask);
		}
		return true;
	}

	public bool TryToggle(bool active, int mode, ulong capability, out NativeTarget target, out string reason)
	{
		target = default(NativeTarget);
		reason = null;
		if (capability == 0L || (capability & (capability - 1)) != 0L)
		{
			reason = "Invalid atomic capability bit.";
			return false;
		}
		if (!Modes.Values.Any((ulong m) => (m & capability) != 0))
		{
			reason = "Capability absent from native modes.";
			return false;
		}
		if (!TryCurrent(active, mode, out var mask))
		{
			reason = "Current native mode is not mapped.";
			return false;
		}
		ulong num = mask ^ capability;
		ulong num2 = num;
		if ((mask & capability) != 0L && (capability & RequiredWhileActiveMask) != 0L && num != 0L)
		{
			reason = "This capability is required by every active native mode; turn off the other functions first.";
			return false;
		}
		if ((mask & capability) == 0L)
		{
			foreach (ulong exclusiveGroup in ExclusiveGroups)
			{
				if ((exclusiveGroup & capability) != 0L)
				{
					num &= ~(exclusiveGroup & ~capability);
				}
			}
		}
		if (num == 0L)
		{
			target = new NativeTarget(active: false, Modes.ContainsKey(mode) ? mode : Modes.Keys.First(), 0uL, 0uL, 0uL);
			return true;
		}
		num |= RequiredWhileActiveMask;
		ulong automaticallyEnabled = num & ~num2;
		ulong automaticallyDisabled = num2 & ~num;
		if (Modes.TryGetValue(mode, out var value) && value == num)
		{
			target = new NativeTarget(active: true, mode, num, automaticallyEnabled, automaticallyDisabled);
			return true;
		}
		foreach (KeyValuePair<int, ulong> mode2 in Modes)
		{
			if (mode2.Value == num)
			{
				target = new NativeTarget(active: true, mode2.Key, num, automaticallyEnabled, automaticallyDisabled);
				return true;
			}
		}
		reason = "No native mode exactly matches the requested capability set.";
		return false;
	}
}
