using FirearmController = EFT.Player.FirearmController;
using System.Collections.Generic;
using EFT;
using EFT.InventoryLogic;
using TacticalDeviceWheel.Compatibility;
using TacticalDeviceWheel.Core;
using UnityEngine;

namespace TacticalDeviceWheel.Devices;

internal sealed class DeviceController
{
	private readonly Plugin plugin;

	public DeviceController(Plugin plugin)
	{
		this.plugin = plugin;
	}

	public bool Toggle(Player player, FirearmController firearm, TacticalCapability capability)
	{
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		if (!NativeSettings.IsToggle || capability == null || (object)player == null || !player.IsYourPlayer || player.HandsController != firearm || player.HealthController == null || !player.HealthController.IsAlive)
		{
			return false;
		}
		Dictionary<string, LightComponent> currentLightStatus = firearm.GetCurrentLightStatus();
		TacticalDevice device = capability.Device;
		if (!currentLightStatus.TryGetValue(device.InstanceId, out var value) || value != device.Component || value._template == null || value._template.ModesCount != device.ModeCount)
		{
			return false;
		}
		if (capability.TargetKind == "Strobe")
		{
			return plugin.Strobes.Toggle(player, firearm, device);
		}
		bool flag = capability.Type == "WhiteLight" && plugin.Strobes.IsRunning(device.InstanceId);
		if (plugin.Strobes.HasSession(device.InstanceId) && !plugin.Strobes.StopDevice(device.InstanceId))
		{
			return false;
		}
		if (flag && capability.IsOn)
		{
			return true;
		}
		NativeTarget target;
		if (capability.TargetKind == "NativeMode")
		{
			target = new NativeTarget(!value.IsActive || value.SelectedMode != capability.Mode, capability.Mode, 0uL, 0uL, 0uL);
		}
		else
		{
			if (capability.TargetKind != "CapabilitySet" || device.ModeMap == null)
			{
				return false;
			}
			if (!device.ModeMap.TryToggle(value.IsActive, value.SelectedMode, capability.Mask, out target, out var reason))
			{
				plugin.Warning("Capability toggle rejected: " + capability.UniqueId + ". " + reason);
				return false;
			}
		}
		if (target.Mode < 0 || target.Mode >= device.ModeCount)
		{
			return false;
		}
		EFT.LightsState val = default(EFT.LightsState);
		val.Id = device.InstanceId;
		val.LightMode = target.Mode;
		val.IsActive = target.IsActive;
		EFT.LightsState val2 = val;
		bool flag2 = firearm.SetLightsState((EFT.LightsState[])(object)new EFT.LightsState[1] { val2 }, false, true);
		plugin.Debug($"APPLY capability={capability.Type} instance={device.InstanceId} mode={target.Mode} active={target.IsActive} mask=0x{target.CapabilityMask:X} automaticOn=0x{target.AutomaticallyEnabledMask:X} automaticOff=0x{target.AutomaticallyDisabledMask:X} accepted={flag2}");
		if (!flag2)
		{
			plugin.Warning("Current weapon operation declined TDW capability change.");
		}
		return flag2;
	}
}
