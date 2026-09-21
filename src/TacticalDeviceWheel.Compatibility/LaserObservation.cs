using TacticalDeviceWheel.Core;

namespace TacticalDeviceWheel.Compatibility;

internal sealed class LaserObservation
{
	public string Node;

	public string Material;

	public string Shader;

	public string PointMaterial;

	public string PointShader;

	public string Evidence;

	public bool Enabled;

	public bool ActiveWithinMode;

	public EmitterSpectrum Spectrum;

	public float BeamSize;

	public bool UsePointLight;

	public string Fingerprint => Shader + "|" + Material + "|" + PointShader + "|" + PointMaterial;
}
