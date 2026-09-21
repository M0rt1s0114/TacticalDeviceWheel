namespace TacticalDeviceWheel.Core;

public readonly struct VanillaModeSignature
{
	public readonly int Index;

	public readonly int VisibleLasers;

	public readonly int InfraredLasers;

	public readonly int UnknownLasers;

	public readonly int WhiteLights;

	public readonly int InfraredLights;

	public readonly int Rangefinders;

	public readonly bool Ambiguous;

	public readonly string EvidenceError;

	public VanillaModeSignature(int index, int visible, int infrared, int unknown, int white, int irLight, bool ambiguous = false, int rangefinders = 0, string evidenceError = null)
	{
		Index = index;
		VisibleLasers = visible;
		InfraredLasers = infrared;
		UnknownLasers = unknown;
		WhiteLights = white;
		InfraredLights = irLight;
		Ambiguous = ambiguous;
		Rangefinders = rangefinders;
		EvidenceError = evidenceError;
	}
}
