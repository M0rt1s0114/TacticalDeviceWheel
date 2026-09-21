using System.Collections.Generic;

namespace TacticalDeviceWheel.Core;

public static class X400ModeResolver
{
	public static bool TryResolve(string template, int modeCount, IEnumerable<ModeSignature> signatures, ulong whiteBit, ulong laserBit, out NativeModeMap map, out string reason)
	{
		map = null;
		reason = "X400 visual layout could not be verified.";
		if (template != "56def37dd2720bec348b456a" || modeCount != 3 || signatures == null || whiteBit == 0L || laserBit == 0L || (whiteBit & (whiteBit - 1)) != 0L || (laserBit & (laserBit - 1)) != 0L || whiteBit == laserBit)
		{
			return false;
		}
		Dictionary<int, ulong> dictionary = new Dictionary<int, ulong>();
		HashSet<ulong> hashSet = new HashSet<ulong>();
		foreach (ModeSignature signature in signatures)
		{
			if (signature.Ambiguous || signature.Index < 0 || signature.Index >= 3 || dictionary.ContainsKey(signature.Index) || signature.Lasers < 0 || signature.SpotLights < 0)
			{
				return false;
			}
			ulong num = ((signature.Lasers > 0) ? laserBit : 0) | ((signature.SpotLights > 0) ? whiteBit : 0);
			if (num == 0L || !hashSet.Add(num))
			{
				return false;
			}
			dictionary.Add(signature.Index, num);
		}
		if (dictionary.Count != 3 || !hashSet.Contains(whiteBit) || !hashSet.Contains(laserBit) || !hashSet.Contains(whiteBit | laserBit))
		{
			return false;
		}
		map = new NativeModeMap(dictionary, 0uL);
		reason = null;
		return true;
	}
}
