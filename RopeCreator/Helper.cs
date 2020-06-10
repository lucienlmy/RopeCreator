using GTA;
using GTA.Math;

namespace RopeCreator
{
	class Helper
	{
		//vehicle and ped bones
		readonly static int[] BONE_INDICES = { 0, 1, 2, 3, 4, 5, 6,
			7, 8, 9, 10, 11, 12, 13, 14, 19, 20, 21, 22, 23, 24,
			25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38,
			39, 40, 41, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53,
			54, 55, 56, 57, 58, 59, 60, 61, 62, 64, 65, 66, 67,
			1356, 2108, 2992, 4089, 4090, 4137, 4138, 4153, 4154,
			4169, 4170, 4185, 4186, 5232, 6286, 6442, 10706, 11174,
			11816, 12844, 14201, 16335, 17188, 17719, 18905, 19336,
			20178, 20279, 20623, 20781, 21550, 22711, 23553, 23639,
			24806, 24816, 24817, 24818, 25260, 26610, 26611, 26612,
			26613, 26614, 27474, 28252, 28422, 29868, 31086, 35502,
			35731, 36029, 36864, 37119, 37193, 39317, 40269, 43536,
			43810, 45509, 45750, 46078, 46240, 47419, 47495, 49979,
			51826, 52301, 56604, 57005, 57597, 57717, 58271, 58331,
			58866, 58867, 58868, 58869, 58870, 60309, 61007, 61163,
			61839, 63931, 64016, 64017, 64064, 64065, 64080, 64081,
			64096, 64097, 64112, 64113, 64729, 65068, 65245 };

		internal static int GetClosestBoneIndex(Entity entity, Vector3 position)
		{
			int closestBoneIndex = 0;
			float closestDistance = float.MaxValue;

			foreach (int boneIndex in BONE_INDICES)
			{
				Vector3 bonePos = entity.GetBoneCoord(boneIndex);
				float distance = position.DistanceTo(bonePos);

				if (distance < closestDistance)
				{
					closestBoneIndex = boneIndex;
					closestDistance = distance;
				}
			}

			return closestBoneIndex;
		}

		internal static bool AreControlsJustPressed(GTA.Control[] controls)
		{
			if (controls.Length == 0) return false;

			for (int i = 0; i < controls.Length; i++)
			{
				if (i == controls.Length - 1 && !Game.IsControlJustPressed(0, controls[i])) return false;
				else if (!Game.IsControlPressed(0, controls[i])) return false;
			}

			return true;
		}

		internal static bool IsAiming()
		{
			return Game.IsControlPressed(0, GTA.Control.Aim) || Game.IsControlPressed(0, GTA.Control.AccurateAim) || Game.IsControlPressed(0, GTA.Control.VehicleAim) || Game.IsControlPressed(0, GTA.Control.VehiclePassengerAim);
		}

		internal static RaycastResult CreateRaycast(Vector3 source, Vector3 target)
		{
			Ped plrPed = Game.Player.Character;

			if (INI.allowIntersectCurrentCar && plrPed.IsInVehicle()) return World.Raycast(source, target, IntersectOptions.Everything);
			else return World.Raycast(source, target, IntersectOptions.Everything, Game.Player.Character);
		}

		internal static RaycastResult CreateRaycastFromCam()
		{
			Vector3 camPos = Vector3.Zero, camOffsetPos = Vector3.Zero;

			Camera cam = World.RenderingCamera;

			if (cam != null && cam.Exists())
			{
				camPos = cam.Position;
				camOffsetPos = cam.GetOffsetInWorldCoords(INI.camOffset);
			}
			else
			{
				camPos = GameplayCamera.Position;
				camOffsetPos = GameplayCamera.GetOffsetInWorldCoords(INI.camOffset);
			}

			return CreateRaycast(camPos, camOffsetPos);
		}
	}
}
