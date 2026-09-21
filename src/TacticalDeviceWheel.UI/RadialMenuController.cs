using FirearmController = EFT.Player.FirearmController;
using System.Collections.Generic;
using EFT;
using EFT.InventoryLogic;
using TacticalDeviceWheel.Core;
using TacticalDeviceWheel.Devices;
using UnityEngine;

namespace TacticalDeviceWheel.UI;

internal sealed class RadialMenuController
{
	private readonly Plugin plugin;

	private readonly WeaponDeviceScanner scanner;

	private readonly DeviceController devices;

	private readonly SelectionMemory selection = new SelectionMemory();

	private List<TacticalCapability> entries = new List<TacticalCapability>();

	private readonly List<RangeReadout> readouts = new List<RangeReadout>();

	private Vector2 vector;

	private FirearmController firearm;

	public bool IsOpen { get; private set; }

	public bool HasSelection
	{
		get
		{
			if (selection.LastValidSelection >= 0)
			{
				return selection.LastValidSelection < entries.Count;
			}
			return false;
		}
	}

	public RadialMenuController(Plugin plugin)
	{
		this.plugin = plugin;
		scanner = new WeaponDeviceScanner(plugin);
		devices = new DeviceController(plugin);
	}

	public void Open(Player player, FirearmController controller)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		Close();
		firearm = controller;
		entries = scanner.Scan(controller);
		vector = Vector2.zero;
		selection.Reset();
		foreach (TacticalCapability entry in entries)
		{
			if (entry.Type == "Rangefinder" && entry.Device.RangeReadout != null)
			{
				readouts.Add(entry.Device.RangeReadout);
			}
		}
		PollReadouts();
		plugin.UI.Show(entries, DisplayName.ParseDisplayName(EFT.LocalizationExtensions.Localized(((Item)controller.Item).ShortName, (string)null)));
		IsOpen = true;
	}

	public void Move(float x, float y)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		if (IsOpen)
		{
			PollReadouts();
			vector = Vector2.ClampMagnitude(vector + new Vector2(x, y) * plugin.Options.Sensitivity.Value, 1f);
			int selected = selection.Update(vector.x, vector.y, entries.Count, plugin.Options.DeadZone.Value);
			plugin.UI.Refresh(selected, vector);
		}
	}

	public bool Commit(Player player, FirearmController controller)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		if (!IsOpen || firearm != controller || !HasSelection)
		{
			return false;
		}
		bool result = devices.Toggle(player, controller, entries[selection.LastValidSelection]);
		PollReadouts();
		plugin.UI.Refresh(selection.LastValidSelection, vector);
		return result;
	}

	private void PollReadouts()
	{
		double realtimeSinceStartupAsDouble = Time.realtimeSinceStartupAsDouble;
		foreach (RangeReadout readout in readouts)
		{
			readout.Poll(realtimeSinceStartupAsDouble, plugin.Options.EnableRangeReadout.Value, plugin.Options.RangeReadoutFrequency.Value);
		}
	}

	public void Close()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		IsOpen = false;
		firearm = null;
		entries.Clear();
		readouts.Clear();
		selection.Reset();
		vector = Vector2.zero;
		plugin.UI?.Hide();
	}
}
