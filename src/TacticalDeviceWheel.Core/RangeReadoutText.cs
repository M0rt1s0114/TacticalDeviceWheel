using System.Text.RegularExpressions;

namespace TacticalDeviceWheel.Core;

public static class RangeReadoutText
{
	private static readonly Regex Tags = new Regex("<[^>]*>", RegexOptions.CultureInvariant);

	private static readonly Regex Number = new Regex("^[0-9]+([.,][0-9]+)?$", RegexOptions.CultureInvariant);

	public static string Format(string nativeText)
	{
		if (string.IsNullOrEmpty(nativeText) || nativeText.Length > 1024)
		{
			return "无读数";
		}
		string text = Tags.Replace(nativeText, "").Trim();
		if (text.Length > 12 || !Number.IsMatch(text))
		{
			return "无读数";
		}
		return text + " 米";
	}
}
