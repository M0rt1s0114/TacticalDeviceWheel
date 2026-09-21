namespace TacticalDeviceWheel.Core;

public sealed class NativeLightObservation
{
	public string Node;

	public string Type;

	public bool Enabled;

	public bool ActiveWithinMode;

	public bool LaserOwned;

	public bool InfraredByIkLight;

	public float Intensity;

	public float Range;

	public float SpotAngle;

	public float[] Color;
}
