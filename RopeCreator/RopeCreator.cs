using System;
using System.Windows.Forms;
using GTA;
using GTA.Math;

namespace RopeCreator;

public class RopeCreator : Script
{
	internal static RopeGroup[] ropeGroups = new RopeGroup[5]
	{
		new RopeGroup(),
		new RopeGroup(),
		new RopeGroup(),
		new RopeGroup(),
		new RopeGroup()
	};

	private int nextDeleteBadRopes;

	private int nextReattachRopes;

	private Vector3 firstPos = Vector3.Zero;

	private Vector3 firstOffset = Vector3.Zero;

	private Entity firstEntity;

	private bool firstAttachBone;

	public RopeCreator()
	{
		INI.GetSettings(base.Settings);
		base.Aborted += RopeCreator_Aborted;
		base.KeyDown += RopeCreator_KeyDown;
		base.Interval = 1;
		base.Tick += RopeCreator_Tick;
	}

	internal static void DeleteAllRopes()
	{
		RopeGroup[] array = ropeGroups;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].DeleteRopes();
		}
	}

	internal static void DeleteLastRope(bool showSubtitle = false)
	{
		RopeGroup ropeGroup = ropeGroups[Menu.liGroupIndex.SelectedIndex];
		if (ropeGroup.ropes.Count > 0)
		{
			int index = ropeGroup.ropes.Count - 1;
			ropeGroup.ropes[index].Delete();
			ropeGroup.ropes.RemoveAt(index);
			UI.ShowSubtitle("Deleted last rope");
		}
	}

	internal static bool AreAllRopesWinding()
	{
		bool result = false;
		RopeGroup[] array = ropeGroups;
		foreach (RopeGroup ropeGroup in array)
		{
			if (ropeGroup.ropes.Count != 0)
			{
				result = true;
				if (!ropeGroup.AreAllRopesWinding())
				{
					return false;
				}
			}
		}
		return result;
	}

	internal static bool AreAllRopesUnwinding()
	{
		bool result = false;
		RopeGroup[] array = ropeGroups;
		foreach (RopeGroup ropeGroup in array)
		{
			if (ropeGroup.ropes.Count != 0)
			{
				result = true;
				if (!ropeGroup.AreAllRopesUnwinding())
				{
					return false;
				}
			}
		}
		return result;
	}

	internal static void StartWindAllRopes()
	{
		RopeGroup[] array = ropeGroups;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].StartWindRopes();
		}
	}

	internal static void StartUnwindAllRopes()
	{
		RopeGroup[] array = ropeGroups;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].StartUnwindRopes();
		}
	}

	internal static void StopWindAllRopes()
	{
		RopeGroup[] array = ropeGroups;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].StopWindRopes();
		}
	}

	internal static void StopUnwindAllRopes()
	{
		RopeGroup[] array = ropeGroups;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].StopUnwindRopes();
		}
	}

	private void DeleteRopesWithBadEntity()
	{
		if (Game.GameTime < nextDeleteBadRopes)
		{
			return;
		}
		bool flag = false;
		RopeGroup[] array = ropeGroups;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].DeleteRopesWithBadEntity())
			{
				flag = true;
			}
			Script.Yield();
		}
		if (flag)
		{
			Menu.ReloadRopeIndices();
		}
		nextDeleteBadRopes = Game.GameTime + 1000;
	}

	private void ReattachRagdollPeds()
	{
		if (Game.GameTime >= nextReattachRopes)
		{
			RopeGroup[] array = ropeGroups;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].ReattachRagdollPeds();
			}
			nextReattachRopes = Game.GameTime + 500;
		}
	}

	private void AttachRope()
	{
		if (INI.onlyCreateRopeWhenAiming && !Helper.IsAiming())
		{
			return;
		}
		RaycastResult raycastResult = Helper.CreateRaycastFromCam();
		if (!Menu.attachNothing && !raycastResult.DitHitAnything)
		{
			return;
		}
		if (firstPos == Vector3.Zero)
		{
			if (!raycastResult.DitHitAnything)
			{
				Camera renderingCamera = World.RenderingCamera;
				if (renderingCamera != null && renderingCamera.Exists())
				{
					firstPos = renderingCamera.GetOffsetInWorldCoords(INI.camOffset);
				}
				else
				{
					firstPos = GameplayCamera.GetOffsetInWorldCoords(INI.camOffset);
				}
			}
			else
			{
				firstPos = raycastResult.HitCoords;
				if (raycastResult.DitHitEntity && raycastResult.HitEntity != null && raycastResult.HitEntity.Exists())
				{
					firstEntity = raycastResult.HitEntity;
					firstOffset = firstEntity.GetOffsetFromWorldCoords(firstPos);
					firstAttachBone = !firstEntity.Model.IsVehicle && ((Menu.attachPedBone && firstEntity.Model.IsPed) || (Menu.attachObjBone && !firstEntity.Model.IsPed));
				}
			}
			UI.ShowSubtitle("First position");
			return;
		}
		Entity entity = null;
		bool secondAttachBone = false;
		Vector3 secondPos;
		if (!raycastResult.DitHitAnything)
		{
			Camera renderingCamera2 = World.RenderingCamera;
			secondPos = ((!(renderingCamera2 != null) || !renderingCamera2.Exists()) ? GameplayCamera.GetOffsetInWorldCoords(INI.camOffset) : renderingCamera2.GetOffsetInWorldCoords(INI.camOffset));
		}
		else
		{
			secondPos = raycastResult.HitCoords;
			if (raycastResult.DitHitEntity)
			{
				entity = raycastResult.HitEntity;
				if (entity != null && entity.Exists())
				{
					secondAttachBone = !entity.Model.IsVehicle && ((Menu.attachPedBone && entity.Model.IsPed) || (Menu.attachObjBone && !entity.Model.IsPed));
				}
			}
		}
		if (firstEntity != null && firstEntity.Exists())
		{
			firstPos = firstEntity.GetOffsetInWorldCoords(firstOffset);
		}
		AttachedRope item = new AttachedRope(firstEntity, firstPos, firstAttachBone, entity, secondPos, secondAttachBone);
		RopeGroup ropeGroup = ropeGroups[Menu.liGroupIndex.SelectedIndex];
		ropeGroup.ropes.Add(item);
		if (ropeGroup.ropes.Count > INI.maxRopes)
		{
			ropeGroup.ropes[0].Delete();
			ropeGroup.ropes.RemoveAt(0);
		}
		Menu.ReloadRopeIndices(selectLast: true);
		firstPos = Vector3.Zero;
		firstEntity = null;
		UI.ShowSubtitle("Created rope");
	}

	private void HandleControls()
	{
		if (Game.CurrentInputMode != InputMode.GamePad)
		{
			return;
		}
		if (Helper.AreControlsJustPressed(INI.menuToggleControls))
		{
			Menu.Toggle();
		}
		if (Menu.modEnabled)
		{
			if (Helper.AreControlsJustPressed(INI.removeLastControls))
			{
				_ = ropeGroups[Menu.liGroupIndex.SelectedIndex];
				DeleteLastRope(showSubtitle: true);
				Menu.ReloadRopeIndices();
			}
			else if (Game.IsControlJustPressed(0, INI.attachControl))
			{
				AttachRope();
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
				_ = ropeGroups[Menu.liGroupIndex.SelectedIndex];
				DeleteLastRope(showSubtitle: true);
				Menu.ReloadRopeIndices();
			}
		}
	}
}
