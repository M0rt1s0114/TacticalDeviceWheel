namespace TacticalDeviceWheel.Core;

public readonly struct ModeSignature
{
	public readonly int Index;

	public readonly int Lasers;

	public readonly int SpotLights;

	public readonly bool Ambiguous;

	public ModeSignature(int index, int lasers, int spots, bool ambiguous)
	{
		Index = index;
		Lasers = lasers;
		SpotLights = spots;
		Ambiguous = ambiguous;
	}
}
