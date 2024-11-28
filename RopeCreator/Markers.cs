using System;
using System.Collections.Generic;
using System.Drawing;
using GTA;
using GTA.Math;

namespace RopeCreator;

internal class Markers : Script
{
	private readonly Vector3 aimMarkerScale = new Vector3(0.05f, 0.05f, 0.05f);

	private readonly Vector3 ropeMarkerScale = new Vector3(0.2f, 0.2f, 0.2f);

	private Vector3 aimMarkerPos = Vector3.Zero;

	private List<Vector3> ropeMarkerPositions = new List<Vector3>();

	private int nextGetMarkerPos;

	public Markers()
	{
		base.Interval = 0;
		base.Tick += Markers_Tick;
	}

	private void GetAimMarkerPos()
	{
		if (Menu.showAimMarker)
		{
			aimMarkerPos = Helper.CreateRaycastFromCam().HitCoords;
		}
	}

	private void GetEditRopeMarkerPos()
	{
		ropeMarkerPositions.Clear();
		if (!Menu.showEditMarkers || ((!Menu.mainMenu.Visible || Menu.mainMenu.SelectedItem != Menu.liGroupIndex) && !Menu.editGroupMenu.Visible && !Menu.editRopeMenu.Visible))
		{
			return;
		}
		RopeGroup ropeGroup = RopeCreator.ropeGroups[Menu.liGroupIndex.SelectedIndex];
		if ((Menu.mainMenu.Visible && Menu.mainMenu.SelectedItem == Menu.liGroupIndex) || Menu.editGroupMenu.Visible)
		{
			if (ropeGroup.ropes.Count <= 0)
			{
				return;
			}
			{
				foreach (AttachedRope rope in ropeGroup.ropes)
				{
					ropeMarkerPositions.Add(rope.firstEntity.GetOffsetInWorldCoords(rope.firstOffset));
					ropeMarkerPositions.Add(rope.secondEntity.GetOffsetInWorldCoords(rope.secondOffset));
				}
				return;
			}
		}
		int selectedItem = Menu.liRopeIndex.SelectedItem;
		if (selectedItem > -1)
		{
			AttachedRope attachedRope = ropeGroup.ropes[selectedItem];
			ropeMarkerPositions.Add(attachedRope.firstEntity.GetOffsetInWorldCoords(attachedRope.firstOffset));
			ropeMarkerPositions.Add(attachedRope.secondEntity.GetOffsetInWorldCoords(attachedRope.secondOffset));
		}
	}

	private void GetAllMarkerPos()
	{
		if (Game.GameTime >= nextGetMarkerPos)
		{
			GetAimMarkerPos();
			GetEditRopeMarkerPos();
			nextGetMarkerPos = Game.GameTime + 10;
		}
	}

	private void DrawMarkers()
	{
		if (!Game.IsScreenFadedIn || !Game.Player.CanControlCharacter || !Menu.modEnabled)
		{
			return;
		}
		if (Menu.showAimMarker && (!INI.onlyShowMarkerWhenAiming || Helper.IsAiming()))
		{
			World.DrawMarker(MarkerType.DebugSphere, aimMarkerPos, Vector3.Zero, Vector3.Zero, aimMarkerScale, Color.Blue);
		}
		if (!Menu.showEditMarkers || ropeMarkerPositions.Count <= 0)
		{
			return;
		}
		foreach (Vector3 ropeMarkerPosition in ropeMarkerPositions)
		{
			World.DrawMarker(MarkerType.DebugSphere, ropeMarkerPosition, Vector3.Zero, Vector3.Zero, ropeMarkerScale, Color.Purple);
		}
	}

	private void Markers_Tick(object sender, EventArgs e)
	{
		GetAllMarkerPos();
		DrawMarkers();
	}
}
