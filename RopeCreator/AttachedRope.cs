using GTA;
using GTA.Math;
using GTA.Native;
using System;

namespace RopeCreator
{
	class AttachedRope
	{
		internal Rope rope;
		internal Entity firstEntity, secondEntity;
		internal Vector3 firstOffset, secondOffset;
		internal float length;
		internal bool deleteFirstEntity = false, deleteSecondEntity = false;
		internal bool winding = false, unwinding = false;

		internal AttachedRope(Entity firstEntity, Vector3 firstPos, Entity secondEntity, Vector3 secondPos)
		{
			if (firstEntity == null || !firstEntity.Exists())
			{
				//use a frozen prop to attach to the world
				firstEntity = CreateAttachProp(firstPos);
				deleteFirstEntity = true;

			}
			else if (!firstEntity.Model.IsVehicle)
			{
				if ((Menu.attachPedBone && firstEntity.Model.IsPed) || (Menu.attachObjBone && !firstEntity.Model.IsPed))
				{
					//use a prop to attach to peds (and objects if wanted)
					//using a prop makes the entity unmoveable via rope (e.g. cars can't move peds)
					var attachProp = CreateAttachProp(Vector3.Zero, false);
					attachProp.AttachTo(firstEntity, Helper.GetClosestBoneIndex(firstEntity, firstPos), firstPos, Vector3.Zero);

					firstEntity = attachProp;
					deleteFirstEntity = true;
				}
			}

			if (secondEntity == null || !secondEntity.Exists())
			{
				secondEntity = CreateAttachProp(secondPos);
				deleteSecondEntity = true;
			}
			else if (!secondEntity.Model.IsVehicle)
			{
				if ((Menu.attachPedBone && secondEntity.Model.IsPed) || (Menu.attachObjBone && !secondEntity.Model.IsPed))
				{
					var attachProp = CreateAttachProp(Vector3.Zero, false);
					attachProp.AttachTo(secondEntity, Helper.GetClosestBoneIndex(secondEntity, secondPos), secondPos, Vector3.Zero);

					secondEntity = attachProp;
					deleteSecondEntity = true;
				}
			}

			float distance = firstPos.DistanceTo(secondPos) + Menu.slack;

			Rope rope = World.AddRope((RopeType)Menu.type, firstPos, DirectionToRotation(secondPos - firstPos, 0), distance, Math.Min(Menu.minLength, distance), Menu.breakable);
			
			rope.AttachEntities(firstEntity, firstPos, secondEntity, secondPos, distance);
			rope.ActivatePhysics();

			this.rope = rope;
			this.firstEntity = firstEntity;
			this.firstOffset = firstEntity.GetOffsetFromWorldCoords(firstPos);
			this.secondEntity = secondEntity;
			this.secondOffset = secondEntity.GetOffsetFromWorldCoords(secondPos);
			this.length = distance;
		}

		private float RadiansToDegrees(float radian)
		{
			return radian * (float)(180.0 / Math.PI);
		}
		
		private Vector3 DirectionToRotation(Vector3 dir, float roll)
		{
			dir = Vector3.Normalize(dir);
			Vector3 rotval;
			rotval.Z = -RadiansToDegrees((float)Math.Atan2(dir.X, dir.Y));
			Vector3 rotpos = Vector3.Normalize(new Vector3(dir.Z, new Vector3(dir.X, dir.Y, 0.0f).Length(), 0.0f));
			rotval.X = RadiansToDegrees((float)Math.Atan2(rotpos.X, rotpos.Y));
			rotval.Y = roll;

			return rotval;
		}

		private Prop CreateAttachProp(Vector3 pos, bool freeze = true)
		{
			Prop prop = World.CreateProp("prop_ashtray_01", pos, true, false);
			
			prop.FreezePosition = freeze;
			prop.IsVisible = false;

			return prop;
		}

		internal void StartWind()
		{
			Function.Call(Hash.START_ROPE_WINDING, rope.Handle);
			winding = true;
		}

		internal void StopWind()
		{
			Function.Call(Hash.STOP_ROPE_WINDING, rope.Handle);
			winding = false;
		}

		internal void StartUnwind()
		{
			Function.Call(Hash.START_ROPE_UNWINDING_FRONT, rope.Handle);
			unwinding = true;
		}

		internal void StopUnwind()
		{
			Function.Call(Hash.STOP_ROPE_UNWINDING_FRONT, rope.Handle);
			unwinding = false;
		}

		internal void Reattach()
		{
			if (rope == null || !rope.Exists()) return;
			if (firstEntity == null || !firstEntity.Exists()) return;
			if (secondEntity == null || !secondEntity.Exists()) return;

			Vector3 firstPos = firstEntity.GetOffsetInWorldCoords(firstOffset),
				secondPos = secondEntity.GetOffsetInWorldCoords(secondOffset);

			rope.AttachEntities(firstEntity, firstPos, secondEntity, secondPos, length);
			rope.ActivatePhysics();
		}

		internal void Delete()
		{
			if (rope != null && rope.Exists())
			{
				rope.DetachEntity(firstEntity);
				rope.DetachEntity(secondEntity);
				rope.Delete();
			}

			rope = null;

			if (deleteFirstEntity)
			{
				if (firstEntity != null && firstEntity.Exists()) firstEntity.Delete();
				firstEntity = null;
			}

			if (deleteSecondEntity)
			{
				if (firstEntity != null && firstEntity.Exists()) secondEntity.Delete();
				secondEntity = null;
			}
		}
	}
}
