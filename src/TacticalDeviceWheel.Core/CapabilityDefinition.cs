namespace TacticalDeviceWheel.Core;

public sealed class CapabilityDefinition
{
	public string Id { get; set; }

	public string Type { get; set; }

	public string Label { get; set; }

	public string LabelEn { get; set; }

	public string IconKey { get; set; }

	public int? Mode { get; set; }

	public string TargetKind { get; set; }
}
