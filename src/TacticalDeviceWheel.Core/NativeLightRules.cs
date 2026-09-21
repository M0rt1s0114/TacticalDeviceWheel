using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TacticalDeviceWheel.Core;

public static class NativeLightRules
{
	private static readonly Regex IndicatorPath = new Regex("^mode_[0-9]{3}/indicator_[0-9]{3}/light$", RegexOptions.CultureInvariant);

	public static bool IsStatusIndicator(NativeLightObservation light, bool enabledForProfile)
	{
		if (enabledForProfile && light != null && light.Type == "Point" && !light.LaserOwned && !light.InfraredByIkLight && light.Range > 0f && light.Range <= 0.02f && light.Node != null)
		{
			return IndicatorPath.IsMatch(light.Node);
		}
		return false;
	}

	public static NativeLightSummary Summarize(IEnumerable<NativeLightObservation> lights, bool ignoreStatusIndicators)
	{
		NativeLightSummary nativeLightSummary = new NativeLightSummary();
		if (lights == null)
		{
			nativeLightSummary.Unsupported.Add("Missing Light observations");
			return nativeLightSummary;
		}
		foreach (NativeLightObservation light in lights)
		{
			if (light == null)
			{
				nativeLightSummary.Unsupported.Add("Null Light observation");
			}
			else if (light.Enabled && light.ActiveWithinMode && !light.LaserOwned)
			{
				if (IsStatusIndicator(light, ignoreStatusIndicators))
				{
					nativeLightSummary.IgnoredIndicators.Add(light.Node);
				}
				else if (light.Type != "Spot")
				{
					nativeLightSummary.Unsupported.Add(light.Type + ":" + light.Node);
				}
				else if (light.InfraredByIkLight)
				{
					nativeLightSummary.InfraredLights++;
				}
				else
				{
					nativeLightSummary.WhiteLights++;
				}
			}
		}
		return nativeLightSummary;
	}
}
