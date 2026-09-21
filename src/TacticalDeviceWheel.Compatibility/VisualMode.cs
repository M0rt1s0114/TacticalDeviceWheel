using System;
using Newtonsoft.Json;
using TacticalDeviceWheel.Core;
using TacticalDeviceWheel.Devices;

namespace TacticalDeviceWheel.Compatibility;

internal sealed class VisualMode
{
	public int Mode;

	public string Node;

	public string Error;

	public string[] ComponentTypes;

	public LaserObservation[] Lasers = Array.Empty<LaserObservation>();

	public NativeLightObservation[] Lights;

	public object[] MaterialDetails;

	public string[] NodePaths;

	public int VisibleLaserCount;

	public int InfraredLaserCount;

	public int UnknownLaserCount;

	public int WhiteLightCount;

	public int InfraredLightCount;

	public int LaserCount;

	public int IndependentSpotLightCount;

	public int UnsupportedLightCount;

	public int RangefinderCount;

	public string[] UnsupportedLights;

	public string[] IgnoredStatusIndicators;

	public object[] Rangefinders;

	[JsonIgnore]
	public RangeDisplaySource[] RangeSources = Array.Empty<RangeDisplaySource>();
}
