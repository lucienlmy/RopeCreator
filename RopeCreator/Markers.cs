using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace RopeCreator
{
	class Markers : Script
	{
		readonly Vector3 aimMarkerScale = new Vector3(0.05f, 0.05f, 0.05f), ropeMarkerScale = new Vector3(0.2f, 0.2f, 0.2f);

		Vector3 aimMarkerPos = Vector3.Zero;
		List<Vector3> ropeMarkerPositions = new List<Vector3>();

		int nextGetMarkerPos = 0;

		public Markers()
		{
			Interval = 0;
			Tick += Markers_Tick;
		}

		private void GetAimMarkerPos()
		{
			if (Menu.showAimMarker)
			{
				var ray = Helper.CreateRaycastFromCam();

				aimMarkerPos = ray.HitCoords;
			}
		}

		private void GetEditRopeMarkerPos()
		{
			ropeMarkerPositions.Clear();

			if (Menu.showEditMarkers)
			{
				if ((Menu.mainMenu.Visible && Menu.mainMenu.SelectedItem == Menu.liGroupIndex) || Menu.editGroupMenu.Visible || Menu.editRopeMenu.Visible)
				{
					var group = RopeCreator.ropeGroups[Menu.liGroupIndex.SelectedIndex];

					if ((Menu.mainMenu.Visible && Menu.mainMenu.SelectedItem == Menu.liGroupIndex) || Menu.editGroupMenu.Visible)
					{
						if (group.ropes.Count > 0)
						{
							foreach (var rope in group.ropes)
							{
								ropeMarkerPositions.Add(rope.firstEntity.GetOffsetInWorldCoords(rope.firstOffset));
								ropeMarkerPositions.Add(rope.secondEntity.GetOffsetInWorldCoords(rope.secondOffset));
							}
						}
					}
					else
					{
						int selectedRopeIndex = Menu.liRopeIndex.SelectedItem;

						if (selectedRopeIndex > -1)
						{
							var selectedRope = group.ropes[selectedRopeIndex];

							ropeMarkerPositions.Add(selectedRope.firstEntity.GetOffsetInWorldCoords(selectedRope.firstOffset));
							ropeMarkerPositions.Add(selectedRope.secondEntity.GetOffsetInWorldCoords(selectedRope.secondOffset));
						}
					}
				}
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
			if (Game.IsScreenFadedIn && Game.Player.CanControlCharacter && Menu.modEnabled)
			{
				if (Menu.showAimMarker && (!INI.onlyShowMarkerWhenAiming || Helper.IsAiming()))
				{
					World.DrawMarker(MarkerType.DebugSphere, aimMarkerPos, Vector3.Zero, Vector3.Zero, aimMarkerScale, Color.Blue);
				}

				if (Menu.showEditMarkers && ropeMarkerPositions.Count > 0)
				{
					foreach (var pos in ropeMarkerPositions)
					{
						World.DrawMarker(MarkerType.DebugSphere, pos, Vector3.Zero, Vector3.Zero, ropeMarkerScale, Color.Purple);
					}
				}
			}
		}

		private void Markers_Tick(object sender, EventArgs e)
		{
			GetAllMarkerPos();
			DrawMarkers();
		}
	}
}
