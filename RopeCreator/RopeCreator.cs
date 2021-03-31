using GTA;
using GTA.Math;
using System;
using System.Windows.Forms;

namespace RopeCreator
{
	public class RopeCreator : Script
	{
		internal static RopeGroup[] ropeGroups = { new RopeGroup(), new RopeGroup(), new RopeGroup(), new RopeGroup(), new RopeGroup() };

		int nextDeleteBadRopes = 0, nextReattachRopes = 0;
		Vector3 firstPos = Vector3.Zero, firstOffset = Vector3.Zero;
		Entity firstEntity = null;
		bool firstAttachBone = false;

		public RopeCreator()
		{
			INI.GetSettings(Settings);

			Aborted += RopeCreator_Aborted;
			KeyDown += RopeCreator_KeyDown;
			Interval = 1;
			Tick += RopeCreator_Tick;
		}

		internal static void DeleteAllRopes()
		{
			foreach (var group in ropeGroups)
			{
				group.DeleteRopes();
			}
		}

		internal static void DeleteLastRope(bool showSubtitle = false)
		{
			var group = ropeGroups[Menu.liGroupIndex.SelectedIndex];

			if (group.ropes.Count > 0)
			{
				int lastIndex = group.ropes.Count - 1;
				group.ropes[lastIndex].Delete();
				group.ropes.RemoveAt(lastIndex);

				UI.ShowSubtitle("Deleted last rope");
			}
		}

		internal static bool AreAllRopesWinding()
		{
			bool oneNotEmpty = false;

			foreach (var group in ropeGroups)
			{
				if (group.ropes.Count == 0) continue; //skip empty groups

				oneNotEmpty = true;

				if (!group.AreAllRopesWinding()) return false;
			}

			//only return true if there is at least one rope
			return oneNotEmpty;
		}

		internal static bool AreAllRopesUnwinding()
		{
			bool oneNotEmpty = false;

			foreach (var group in ropeGroups)
			{
				if (group.ropes.Count == 0) continue; //skip empty groups

				oneNotEmpty = true;

				if (!group.AreAllRopesUnwinding()) return false;
			}

			//only return true if there is at least one rope
			return oneNotEmpty;
		}

		internal static void StartWindAllRopes()
		{
			foreach (var group in ropeGroups)
			{
				group.StartWindRopes();
			}
		}

		internal static void StartUnwindAllRopes()
		{
			foreach (var group in ropeGroups)
			{
				group.StartUnwindRopes();
			}
		}

		internal static void StopWindAllRopes()
		{
			foreach (var group in ropeGroups)
			{
				group.StopWindRopes();
			}
		}

		internal static void StopUnwindAllRopes()
		{
			foreach (var group in ropeGroups)
			{
				group.StopUnwindRopes();
			}
		}

		private void DeleteRopesWithBadEntity()
		{
			if (Game.GameTime >= nextDeleteBadRopes)
			{
				bool deletedOne = false;

				foreach (var group in ropeGroups)
				{
					if (group.DeleteRopesWithBadEntity())
					{
						deletedOne = true;
					}

					Yield();
				}

				if (deletedOne) Menu.ReloadRopeIndices();

				nextDeleteBadRopes = Game.GameTime + 1000;
			}
		}

		private void ReattachRagdollPeds()
		{
			if (Game.GameTime >= nextReattachRopes)
			{
				foreach (var group in ropeGroups)
				{
					group.ReattachRagdollPeds();
				}

				nextReattachRopes = Game.GameTime + 500;
			}
		}

		private void AttachRope()
		{
			if (!INI.onlyCreateRopeWhenAiming || Helper.IsAiming())
			{
				var ray = Helper.CreateRaycastFromCam();

				if (Menu.attachNothing || ray.DitHitAnything)
				{
					if (firstPos == Vector3.Zero)
					{
						if (!ray.DitHitAnything) //attach to nothing
						{
							Camera cam = World.RenderingCamera;

							if (cam != null && cam.Exists()) firstPos = cam.GetOffsetInWorldCoords(INI.camOffset);
							else firstPos = GameplayCamera.GetOffsetInWorldCoords(INI.camOffset);
						}
						else
						{
							firstPos = ray.HitCoords;

							if (ray.DitHitEntity && ray.HitEntity != null && ray.HitEntity.Exists())
							{
								firstEntity = ray.HitEntity;
								firstOffset = firstEntity.GetOffsetFromWorldCoords(firstPos);
								firstAttachBone = !firstEntity.Model.IsVehicle && ((Menu.attachPedBone && firstEntity.Model.IsPed) || (Menu.attachObjBone && !firstEntity.Model.IsPed));
							}
						}

						UI.ShowSubtitle("First position");
					}
					else
					{
						Vector3 secondPos;
						Entity secondEntity = null;
						bool secondAttachBone = false;

						if (!ray.DitHitAnything) //attach to nothing
						{
							Camera cam = World.RenderingCamera;

							if (cam != null && cam.Exists()) secondPos = cam.GetOffsetInWorldCoords(INI.camOffset);
							else secondPos = GameplayCamera.GetOffsetInWorldCoords(INI.camOffset);
						}
						else
						{
							secondPos = ray.HitCoords;

							if (ray.DitHitEntity)
							{
								secondEntity = ray.HitEntity;

								if (secondEntity != null && secondEntity.Exists())
								{
									secondAttachBone = !secondEntity.Model.IsVehicle && ((Menu.attachPedBone && secondEntity.Model.IsPed) || (Menu.attachObjBone && !secondEntity.Model.IsPed));
								}
							}
						}

						if (firstEntity != null && firstEntity.Exists())
						{
							firstPos = firstEntity.GetOffsetInWorldCoords(firstOffset);
						}

						var rope = new AttachedRope(firstEntity, firstPos, firstAttachBone, secondEntity, secondPos, secondAttachBone);

						var group = ropeGroups[Menu.liGroupIndex.SelectedIndex];

						group.ropes.Add(rope);

						if (group.ropes.Count > INI.maxRopes)
						{
							group.ropes[0].Delete();
							group.ropes.RemoveAt(0);
						}

						Menu.ReloadRopeIndices(true); //true = select last item

						firstPos = Vector3.Zero;
						firstEntity = null;

						UI.ShowSubtitle("Created rope");
					}
				}
				//else UI.ShowSubtitle("Didn't hit anything");
			}
		}

		private void HandleControls()
		{
			if (Game.CurrentInputMode == InputMode.GamePad)
			{
				if (Helper.AreControlsJustPressed(INI.menuToggleControls))
				{
					Menu.Toggle();
				}

				if (Menu.modEnabled)
				{
					if (Helper.AreControlsJustPressed(INI.removeLastControls))
					{
						var group = ropeGroups[Menu.liGroupIndex.SelectedIndex];

						DeleteLastRope(true);
						Menu.ReloadRopeIndices();
					}
					else if (Game.IsControlJustPressed(0, INI.attachControl))
					{
						AttachRope();
					}
				}
			}
		}

		private void RopeCreator_Aborted(object sender, EventArgs e)
		{
			DeleteAllRopes();
		}

		private void RopeCreator_Tick(object sender, EventArgs e)
		{
			DeleteRopesWithBadEntity();
			ReattachRagdollPeds();
			HandleControls();
		}

		private void RopeCreator_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == INI.menuToggleKey)
			{
				Menu.Toggle();
			}

			if (Menu.modEnabled)
			{
				if (e.KeyCode == INI.attachKey)
				{
					AttachRope();
				}
				else if (e.KeyCode == INI.removeLastKey)
				{
					var group = ropeGroups[Menu.liGroupIndex.SelectedIndex];

					DeleteLastRope(true);
					Menu.ReloadRopeIndices();
				}
			}
		}
	}
}
