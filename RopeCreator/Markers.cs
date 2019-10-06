using GTA;
using GTA.Math;
using System;
using System.Drawing;

namespace RopeCreator
{
	class Markers : Script
	{
		readonly Vector3 aimMarkerScale = new Vector3(0.05f, 0.05f, 0.05f), ropeMarkerScale = new Vector3(0.2f, 0.2f, 0.2f);

		Vector3 aimMarkerPos = Vector3.Zero;
		Vector3 ropeMarker1Pos = Vector3.Zero, ropeMarker2Pos = Vector3.Zero;

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
			if (Menu.showEditMarkers && Menu.editRopeMenu.Visible)
			{
				int selectedRopeIndex = (int)Menu.liRopeIndex.Items[Menu.liRopeIndex.Index];

				if (selectedRopeIndex > -1)
				{
					var selectedRope = RopeCreator.ropes[selectedRopeIndex];

					ropeMarker1Pos = selectedRope.firstEntity.GetOffsetInWorldCoords(selectedRope.firstOffset);
					ropeMarker2Pos = selectedRope.secondEntity.GetOffsetInWorldCoords(selectedRope.secondOffset);
				}
				else ropeMarker1Pos = ropeMarker2Pos = Vector3.Zero;
			}
			else ropeMarker1Pos = ropeMarker2Pos = Vector3.Zero;
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

				if (Menu.showEditMarkers && ropeMarker1Pos != Vector3.Zero && ropeMarker2Pos != Vector3.Zero)
				{
					World.DrawMarker(MarkerType.DebugSphere, ropeMarker1Pos, Vector3.Zero, Vector3.Zero, ropeMarkerScale, Color.Purple);
					World.DrawMarker(MarkerType.DebugSphere, ropeMarker2Pos, Vector3.Zero, Vector3.Zero, ropeMarkerScale, Color.Purple);
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
