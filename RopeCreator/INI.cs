using GTA;
using GTA.Math;
using LemonUI.Menus;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RopeCreator
{
	static class INI
	{
		internal static GTA.Control attachControl;
		internal static GTA.Control[] removeLastControls, menuToggleControls;
		internal static Keys attachKey, removeLastKey, menuToggleKey;
		internal static bool onlyCreateRopeWhenAiming, onlyShowMarkerWhenAiming, allowIntersectCurrentCar;
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
			bool enabled = settingsFile.GetValue("General", "EnabledOnStartup", true);
			bool showAimMarker = settingsFile.GetValue("General", "ShowAimMarker", true);
			bool showEditMarkers = settingsFile.GetValue("General", "ShowRopeMarkers", true);
			onlyCreateRopeWhenAiming = settingsFile.GetValue("General", "OnlyCreateRopeWhenAiming", true);
			onlyShowMarkerWhenAiming = settingsFile.GetValue("General", "OnlyShowMarkerWhenAiming", true);
			allowIntersectCurrentCar = settingsFile.GetValue("General", "AllowTargetingOfCurrentVehicle", true);
			maxRopes = Math.Max(settingsFile.GetValue("General", "MaxRopes", 15), 1); //lock between 1 and int.MaxValue
			float maxDistance = Math.Min(Math.Max(settingsFile.GetValue("General", "MaxDistance", 50f), 1f), 500f); //lock between 1 and 500
			camOffset = new Vector3(0f, maxDistance, 0f);
			int ropeType = Math.Min(Math.Max(settingsFile.GetValue("Rope", "DefaultType", 1), 1), 5);
			bool breakable = settingsFile.GetValue("Rope", "DefaultBreakable", false);
			bool attachToObjBone = settingsFile.GetValue("Rope", "DefaultAttachToObjectBone", false);
			bool attachToPedBone = settingsFile.GetValue("Rope", "DefaultAttachToPedBone", false);
			bool attachNothing = settingsFile.GetValue("Rope", "DefaultAttachToNothing", false);

			Menu.modEnabled = enabled;
			if (Menu.cbEnabled != default(NativeCheckboxItem))
			{
				Menu.cbEnabled.Checked = enabled;
			}

			Menu.showAimMarker = showAimMarker;
			if (Menu.cbShowAimMarker != default(NativeCheckboxItem))
			{
				Menu.cbShowAimMarker.Checked = showAimMarker;
			}

			Menu.showEditMarkers = showEditMarkers;
			if (Menu.cbShowEditMarkers != default(NativeCheckboxItem))
			{
				Menu.cbShowEditMarkers.Checked = showEditMarkers;
			}

			Menu.type = ropeType;
			if (Menu.liRopeIndex != default(NativeListItem<int>))
			{
				Menu.liRopeIndex.SelectedIndex = ropeType - 1;
			}

			Menu.breakable = breakable;
			if (Menu.cbBreakable != default(NativeCheckboxItem))
			{
				Menu.cbBreakable.Checked = breakable;
			}

			Menu.attachObjBone = attachToObjBone;
			if (Menu.cbAttachObjBone != default(NativeCheckboxItem))
			{
				Menu.cbAttachObjBone.Checked = attachToObjBone;
			}

			Menu.attachPedBone = attachToPedBone;
			if (Menu.cbAttachPedBone != default(NativeCheckboxItem))
			{
				Menu.cbAttachPedBone.Checked = attachToPedBone;
			}

			Menu.attachNothing = attachNothing;
			if (Menu.cbAttachNothing != default(NativeCheckboxItem))
			{
				Menu.cbAttachNothing.Checked = attachNothing;
			}
		}

		private static GTA.Control[] ParseMultiControl(string strControls)
		{
			List<GTA.Control> controls = new List<GTA.Control>();
			string[] strArrControls = strControls.Split('+');

			foreach (var strControl in strArrControls)
			{
				if (string.IsNullOrWhiteSpace(strControl)) continue;

				GTA.Control control;
				bool success = Enum.TryParse(strControl.Trim(), out control);

				if (success) controls.Add(control);
			}

			return controls.ToArray();
		}
	}
}
