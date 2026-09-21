using System;
using System.Collections.Generic;

namespace TacticalDeviceWheel.Core;

public static class VanillaModeResolver
{
	public static bool TryResolve(int count, string laserSpectrum, IEnumerable<VanillaModeSignature> signatures, IReadOnlyList<CapabilityDefinition> capabilities, out NativeModeMap map, out string reason)
	{
		map = null;
		reason = "Invalid mode count, missing signatures or invalid capability definitions.";
		if (count < 1 || count > 64 || signatures == null || capabilities == null || capabilities.Count < 1 || capabilities.Count > 64)
		{
			return false;
		}
		Dictionary<string, ulong> dictionary = new Dictionary<string, ulong>(StringComparer.Ordinal);
		for (int i = 0; i < capabilities.Count; i++)
		{
			string text = capabilities[i]?.Type;
			if (text == null || dictionary.ContainsKey(text))
			{
				return false;
			}
			dictionary.Add(text, (ulong)(1L << i));
		}
		Dictionary<int, ulong> dictionary2 = new Dictionary<int, ulong>();
		foreach (VanillaModeSignature signature in signatures)
		{
			if (signature.Ambiguous)
			{
				reason = "Mode " + signature.Index + ": " + (signature.EvidenceError ?? "ambiguous or unsupported visual components; inspect scan evidence.");
				return false;
			}
			if (signature.Index < 0 || signature.Index >= count || dictionary2.ContainsKey(signature.Index))
			{
				reason = "Invalid or duplicate visual mode index " + signature.Index + "; native ModesCount=" + count;
				return false;
			}
			if (signature.VisibleLasers < 0 || signature.InfraredLasers < 0 || signature.UnknownLasers < 0 || signature.WhiteLights < 0 || signature.InfraredLights < 0 || signature.Rangefinders < 0)
			{
				reason = "Mode " + signature.Index + ": invalid emitter counts.";
				return false;
			}
			int num = signature.VisibleLasers;
			int num2 = signature.InfraredLasers;
			if (signature.UnknownLasers > 0)
			{
				if (laserSpectrum == "VisibleOnly")
				{
					num += signature.UnknownLasers;
				}
				else
				{
					if (!(laserSpectrum == "InfraredOnly"))
					{
						reason = "Mode " + signature.Index + ": laser spectrum has no usable IR/visible evidence.";
						return false;
					}
					num2 += signature.UnknownLasers;
				}
			}
			if ((laserSpectrum == "VisibleOnly" && num2 > 0) || (laserSpectrum == "InfraredOnly" && num > 0))
			{
				reason = "Runtime spectrum conflicts with the vanilla profile.";
				return false;
			}
			ulong num3 = 0uL;
			(string, int)[] array = new(string, int)[5]
			{
				("WhiteLight", signature.WhiteLights),
				("VisibleLaser", num),
				("IRLaser", num2),
				("IRIlluminator", signature.InfraredLights),
				("Rangefinder", signature.Rangefinders)
			};
			for (int j = 0; j < array.Length; j++)
			{
				(string, int) tuple = array[j];
				if (tuple.Item2 != 0)
				{
					if (!dictionary.TryGetValue(tuple.Item1, out var value))
					{
						reason = "Runtime effect is outside this vanilla profile: " + tuple.Item1;
						return false;
					}
					num3 |= value;
				}
			}
			if (num3 == 0L)
			{
				reason = "Mode " + signature.Index + ": no supported emitter was recognized.";
				return false;
			}
			dictionary2.Add(signature.Index, num3);
		}
		if (dictionary2.Count != count)
		{
			reason = "Visual mode count=" + dictionary2.Count + ", native ModesCount=" + count;
			return false;
		}
		map = new NativeModeMap(dictionary2, 0uL);
		reason = null;
		return true;
	}
}
