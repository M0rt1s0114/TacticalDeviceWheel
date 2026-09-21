using System;
using System.Collections.Generic;
using System.Linq;

namespace TacticalDeviceWheel.Core;

public static class NativeControlPolicy
{
	public static bool TryMasks(DeviceDefinition definition, out ulong required, out ulong[] groups, out string reason)
	{
		required = 0uL;
		groups = Array.Empty<ulong>();
		reason = "Invalid native control policy.";
		if (definition?.Capabilities == null || definition.Capabilities.Count > 64)
		{
			return false;
		}
		Dictionary<string, ulong> dictionary = new Dictionary<string, ulong>(StringComparer.Ordinal);
		for (int i = 0; i < definition.Capabilities.Count; i++)
		{
			string text = definition.Capabilities[i]?.Type;
			if (text == null || dictionary.ContainsKey(text))
			{
				return false;
			}
			dictionary.Add(text, (ulong)(1L << i));
		}
		foreach (string item in definition.RequiredWhileActive ?? new List<string>())
		{
			if (item == null || !dictionary.TryGetValue(item, out var value) || (required & value) != 0L)
			{
				return false;
			}
			required |= value;
		}
		List<ulong> list = new List<ulong>();
		foreach (List<string> item2 in definition.ExclusiveGroups ?? new List<List<string>>())
		{
			if (item2 == null || item2.Count < 2 || item2.Count > 64)
			{
				return false;
			}
			ulong num = 0uL;
			foreach (string item3 in item2)
			{
				if (item3 == null || !dictionary.TryGetValue(item3, out var value2) || (num & value2) != 0L)
				{
					return false;
				}
				num |= value2;
			}
			if ((num & required) != 0L)
			{
				reason = "Required native capabilities cannot be mutually exclusive.";
				return false;
			}
			if (!list.Contains(num))
			{
				list.Add(num);
			}
		}
		groups = list.ToArray();
		reason = null;
		return true;
	}

	public static bool TryApply(DeviceDefinition definition, NativeModeMap measured, out NativeModeMap map, out string reason)
	{
		map = null;
		if (!TryMasks(definition, out var required, out var groups, out reason))
		{
			return false;
		}
		if (measured == null || measured.Modes.Count == 0)
		{
			reason = "Native control policy has no measured modes.";
			return false;
		}
		foreach (KeyValuePair<int, ulong> mode in measured.Modes)
		{
			if ((mode.Value & required) != required)
			{
				reason = "Mode " + mode.Key + " does not contain the declared required capabilities.";
				return false;
			}
			ulong[] array = groups;
			foreach (ulong num in array)
			{
				ulong num2 = mode.Value & num;
				if (num2 != 0L && (num2 & (num2 - 1)) != 0L)
				{
					reason = "Mode " + mode.Key + " contradicts a declared exclusive group.";
					return false;
				}
			}
		}
		map = new NativeModeMap(measured.Modes.ToDictionary((KeyValuePair<int, ulong> p) => p.Key, (KeyValuePair<int, ulong> p) => p.Value), required, groups);
		reason = null;
		return true;
	}
}
