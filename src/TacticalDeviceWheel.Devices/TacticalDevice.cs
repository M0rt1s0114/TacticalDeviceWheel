using System.Collections.Generic;
using EFT.InventoryLogic;
using TacticalDeviceWheel.Core;

namespace TacticalDeviceWheel.Devices;

internal sealed class TacticalDevice
{
	public string InstanceId;

	public string TemplateId;

	public string Slot;

	public DisplayName DisplayName;

	public int ModeCount;

	public int Ordinal;

	public LightComponent Component;

	public ItemIcon ItemIcon;

	public NativeModeMap ModeMap;

	public StrobeController Strobes;

	public RangeReadout RangeReadout;

	public bool DatabaseMatched;

	public string MappingSource = "Unknown";

	public string MappingReason;

	public string MappingEvidence;

	public readonly List<TacticalCapability> Capabilities = new List<TacticalCapability>();

	public string Name => DisplayName.PlainText + " [" + Ordinal.ToString("00") + "]";
}
