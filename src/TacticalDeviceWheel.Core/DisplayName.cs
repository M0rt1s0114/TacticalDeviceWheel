using System.Globalization;
using System.Text.RegularExpressions;

namespace TacticalDeviceWheel.Core;

public sealed class DisplayName
{
	private static readonly Regex ColorTag = new Regex("<color\\s*=\\s*[\"']?#([0-9a-f]{8}|[0-9a-f]{6})[\"']?\\s*>", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

	private static readonly Regex Tags = new Regex("<[^>]*>", RegexOptions.CultureInvariant);

	public string RawText { get; private set; }

	public string PlainText { get; private set; }

	public uint? Rgba { get; private set; }

	public string ColorHex
	{
		get
		{
			if (!Rgba.HasValue)
			{
				return null;
			}
			return "#" + Rgba.Value.ToString("X8", CultureInfo.InvariantCulture);
		}
	}

	public static DisplayName ParseDisplayName(string raw)
	{
		if (raw == null)
		{
			raw = "";
		}
		Match match = ColorTag.Match(raw);
		uint? rgba = null;
		if (match.Success && uint.TryParse(match.Groups[1].Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var result))
		{
			rgba = ((match.Groups[1].Length == 6) ? ((result << 8) | 0xFFu) : result);
		}
		return new DisplayName
		{
			RawText = raw,
			PlainText = Tags.Replace(raw, "").Trim(),
			Rgba = rgba
		};
	}
}
