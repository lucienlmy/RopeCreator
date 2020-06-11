using GTA;
using GTA.Math;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RopeCreator
{
	public class RopeCreator : Script
	{
		internal static List<AttachedRope> ropes = new List<AttachedRope>();

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
			if (ropes.Count > 0)
			{
				foreach (var rope in ropes)
				{
					rope.Delete();
				}

				ropes.Clear();
			}
		}

		internal static void DeleteLastRope(bool showSubtitle = false)
		{
			if (ropes.Count > 0)
			{
				int lastIndex = ropes.Count - 1;
				ropes[lastIndex].Delete();
				ropes.RemoveAt(lastIndex);

				UI.ShowSubtitle("Deleted last rope");
			}
		}

		internal static bool AreAllRopesWinding()
		{
			if (ropes.Count == 0) return false;
			
			foreach (var rope in ropes)
			{
				if (!rope.winding) return false;
			}

			return true;
		}

		internal static bool AreAllRopesUnwinding()
		{
			if (ropes.Count == 0) return false;

			foreach (var rope in ropes)
			{
				if (!rope.unwinding) return false;
			}

			return true;
		}

		internal static void StartWindAllRopes()
		{
			if (ropes.Count > 0)
			{
				foreach (var rope in ropes)
				{
					rope.StopUnwind();
					rope.StartWind();
				}
			}
		}

		internal static void StartUnwindAllRopes()
		{
			if (ropes.Count > 0)
			{
				foreach (var rope in ropes)
				{
					rope.StopWind();
					rope.StartUnwind();
				}
			}
		}

		internal static void StopWindAllRopes()
		{
			if (ropes.Count > 0)
			{
				foreach (var rope in ropes)
				{
					rope.StopWind();
				}
			}
		}

		internal static void StopUnwindAllRopes()
		{
			if (ropes.Count > 0)
			{
				foreach (var rope in ropes)
				{
					rope.StopUnwind();
				}
			}
		}

		private void DeleteRopesWithBadEntity()
		{
			if (Game.GameTime >= nextDeleteBadRopes)
			{
				if (ropes.Count > 0)
				{
					for (int i = 0; i < ropes.Count; i++)
					{
						var rope = ropes[i];

						if (rope.firstEntity == null || !rope.firstEntity.Exists() || (rope.firstEntity.Model.IsPed && rope.firstEntity.IsDead) ||
							rope.secondEntity == null || !rope.secondEntity.Exists() || (rope.secondEntity.Model.IsPed && rope.secondEntity.IsDead))
						{
							rope.Delete();
							ropes.RemoveAt(i);
							i--;
						}

						Yield();
					}
				}

				nextDeleteBadRopes = Game.GameTime + 1000;
			}
		}

		private void ReattachRagdollPeds()
		{
			if (Game.GameTime >= nextReattachRopes)
			{
				if (ropes.Count > 0)
				{
					foreach (var rope in ropes)
					{
						if ((rope.firstEntity.Model.IsPed && ((Ped)rope.firstEntity).IsRagdoll) ||
							(rope.secondEntity.Model.IsPed && ((Ped)rope.secondEntity).IsRagdoll))
						{
							rope.Reattach();
						}
					}
				}

				nextReattachRopes = Game.GameTime + 500;
			}
		}

		private void AttachRope()
		{
			if (!INI.onlyCreateRopeWhenAiming || Helper.IsAiming())
			{
				var ray = Helper.CreateRaycastFromCam();

				if (ray.DitHitAnything)
				{
					if (firstPos == Vector3.Zero)
					{
						firstPos = ray.HitCoords;

						if (ray.DitHitEntity && ray.HitEntity != null && ray.HitEntity.Exists())
						{
							firstEntity = ray.HitEntity;
							firstOffset = firstEntity.GetOffsetFromWorldCoords(firstPos);
							firstAttachBone = !firstEntity.Model.IsVehicle && (Menu.attachPedBone && firstEntity.Model.IsPed) || (Menu.attachObjBone && !firstEntity.Model.IsPed);
						}

						UI.ShowSubtitle("First position");
					}
					else
					{
						Vector3 secondPos = ray.HitCoords;
						Entity secondEntity = null;
						bool secondAttachBone = false;

						if (ray.DitHitEntity)
						{
							secondEntity = ray.HitEntity;

							if (secondEntity != null && secondEntity.Exists())
							{
								secondAttachBone = !secondEntity.Model.IsVehicle && (Menu.attachPedBone && secondEntity.Model.IsPed) || (Menu.attachObjBone && !secondEntity.Model.IsPed);
							}
						}

						if (firstEntity != null && firstEntity.Exists())
						{
							firstPos = firstEntity.GetOffsetInWorldCoords(firstOffset);
						}

						var rope = new AttachedRope(firstEntity, firstPos, firstAttachBone, secondEntity, secondPos, secondAttachBone);

						ropes.Add(rope);

						if (ropes.Count > INI.maxRopes)
						{
							ropes[0].Delete();
							ropes.RemoveAt(0);
						}

						Menu.ReloadRopeIndices(ropes.Count, ropes.Count - 1);

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
						DeleteLastRope(true);
						Menu.ReloadRopeIndices(ropes.Count);
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
					DeleteLastRope(true);
					Menu.ReloadRopeIndices(ropes.Count);
				}
			}
		}
	}
}
