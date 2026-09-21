using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace TacticalDeviceWheel.Core;

public static class SpectrumMarkers
{
	private static readonly Regex Ir = new Regex("(^|[^a-z0-9])(ir|ik|infrared|infra_red)([^a-z0-9]|$)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

	private static readonly Regex Vis = new Regex("(^|[^a-z0-9])(visible|vis|red|green|blue)([^a-z0-9]|$)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

	public static EmitterSpectrum Read(params string[] assetNames)
	{
		string[] source = assetNames ?? Array.Empty<string>();
		if (source.Any((string s) => s != null && Ir.IsMatch(s)))
		{
			return EmitterSpectrum.Infrared;
		}
		if (source.Any((string s) => s != null && Vis.IsMatch(s)))
		{
			return EmitterSpectrum.Visible;
		}
		return EmitterSpectrum.Unknown;
	}
}
