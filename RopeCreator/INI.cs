using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GTA;
using GTA.Math;

namespace RopeCreator;

internal static class INI
{
	internal static GTA.Control attachControl;

	internal static GTA.Control[] removeLastControls;

	internal static GTA.Control[] menuToggleControls;

	internal static Keys attachKey;

	internal static Keys removeLastKey;

	internal static Keys menuToggleKey;

	internal static bool onlyCreateRopeWhenAiming;

	internal static bool onlyShowMarkerWhenAiming;

	internal static bool allowIntersectCurrentCar;

	internal static int maxRopes;

	internal static Vector3 camOffset = Vector3.Zero;

	internal static void GetSettings(ScriptSettings settingsFile)
	{
		attachKey = settingsFile.GetValue("Controls", "AttachKey", Keys.E);
		attachControl = settingsFile.GetValue("Controls", "AttachPadButton", GTA.Control.Sprint);
		removeLastKey = settingsFile.GetValue("Controls", "RemoveLastRopeKey", Keys.Z);
		removeLastControls = ParseMultiControl(settingsFile.GetValue("Controls", "RemoveLastRopePadButtons", "ScriptPadLeft"));
		menuToggleKey = settingsFile.GetValue("Controls", "MenuToggleKey", Keys.F6);
		menuToggleControls = ParseMultiControl(settingsFile.GetValue("Controls", "MenuTogglePadButtons", "ScriptRB+Sprint"));
		bool value = settingsFile.GetValue("General", "EnabledOnStartup", defaultvalue: true);
		bool value2 = settingsFile.GetValue("General", "ShowAimMarker", defaultvalue: true);
		bool value3 = settingsFile.GetValue("General", "ShowRopeMarkers", defaultvalue: true);
		onlyCreateRopeWhenAiming = settingsFile.GetValue("General", "OnlyCreateRopeWhenAiming", defaultvalue: true);
		onlyShowMarkerWhenAiming = settingsFile.GetValue("General", "OnlyShowMarkerWhenAiming", defaultvalue: true);
		allowIntersectCurrentCar = settingsFile.GetValue("General", "AllowTargetingOfCurrentVehicle", defaultvalue: true);
		maxRopes = Math.Max(settingsFile.GetValue("General", "MaxRopes", 15), 1);
		float y = Math.Min(Math.Max(settingsFile.GetValue("General", "MaxDistance", 50f), 1f), 500f);
		camOffset = new Vector3(0f, y, 0f);
		int num = Math.Min(Math.Max(settingsFile.GetValue("Rope", "DefaultType", 1), 1), 5);
		bool value4 = settingsFile.GetValue("Rope", "DefaultBreakable", defaultvalue: false);
		bool value5 = settingsFile.GetValue("Rope", "DefaultAttachToObjectBone", defaultvalue: false);
		bool value6 = settingsFile.GetValue("Rope", "DefaultAttachToPedBone", defaultvalue: false);
		bool value7 = settingsFile.GetValue("Rope", "DefaultAttachToNothing", defaultvalue: false);
		Menu.modEnabled = value;
		if (Menu.cbEnabled != null)
		{
			Menu.cbEnabled.Checked = value;
		}
		Menu.showAimMarker = value2;
		if (Menu.cbShowAimMarker != null)
		{
			Menu.cbShowAimMarker.Checked = value2;
		}
		Menu.showEditMarkers = value3;
		if (Menu.cbShowEditMarkers != null)
		{
			Menu.cbShowEditMarkers.Checked = value3;
		}
		Menu.type = num;
		if (Menu.liRopeIndex != null)
		{
			Menu.liRopeIndex.SelectedIndex = num - 1;
		}
		Menu.breakable = value4;
		if (Menu.cbBreakable != null)
		{
			Menu.cbBreakable.Checked = value4;
		}
		Menu.attachObjBone = value5;
		if (Menu.cbAttachObjBone != null)
		{
			Menu.cbAttachObjBone.Checked = value5;
		}
		Menu.attachPedBone = value6;
		if (Menu.cbAttachPedBone != null)
		{
			Menu.cbAttachPedBone.Checked = value6;
		}
		Menu.attachNothing = value7;
		if (Menu.cbAttachNothing != null)
		{
			Menu.cbAttachNothing.Checked = value7;
		}
	}

	private static GTA.Control[] ParseMultiControl(string strControls)
	{
		List<GTA.Control> list = new List<GTA.Control>();
		string[] array = strControls.Split('+');
		foreach (string text in array)
		{
			if (!string.IsNullOrWhiteSpace(text) && Enum.TryParse<GTA.Control>(text.Trim(), out var result))
			{
				list.Add(result);
			}
		}
		return list.ToArray();
	}
}
