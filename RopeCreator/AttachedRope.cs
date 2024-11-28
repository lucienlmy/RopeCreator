using System;
using GTA;
using GTA.Math;
using GTA.Native;

namespace RopeCreator;

internal class AttachedRope
{
	internal RopeGroup group;

	internal Rope rope;

	internal Entity firstEntity;

	internal Entity secondEntity;

	internal Vector3 firstOffset;

	internal Vector3 secondOffset;

	internal float length;

	internal bool deleteFirstEntity;

	internal bool deleteSecondEntity;

	internal bool winding;

	internal bool unwinding;

	internal AttachedRope(Entity firstEntity, Vector3 firstPos, bool firstAttachBone, Entity secondEntity, Vector3 secondPos, bool secondAttachBone)
	{
		if (firstEntity == null || !firstEntity.Exists())
		{
			firstEntity = CreateAttachProp(firstPos);
			deleteFirstEntity = true;
		}
		else if (firstAttachBone)
		{
			Prop prop = CreateAttachProp(Vector3.Zero, freeze: false);
			prop.AttachTo(firstEntity, Helper.GetClosestBoneIndex(firstEntity, firstPos), firstPos, Vector3.Zero);
			firstEntity = prop;
			deleteFirstEntity = true;
		}
		if (secondEntity == null || !secondEntity.Exists())
		{
			secondEntity = CreateAttachProp(secondPos);
			deleteSecondEntity = true;
		}
		else if (secondAttachBone)
		{
			Prop prop2 = CreateAttachProp(Vector3.Zero, freeze: false);
			prop2.AttachTo(secondEntity, Helper.GetClosestBoneIndex(secondEntity, secondPos), secondPos, Vector3.Zero);
			secondEntity = prop2;
			deleteSecondEntity = true;
		}
		float val = firstPos.DistanceTo(secondPos) + Menu.slack;
		Rope rope = World.AddRope((RopeType)Menu.type, firstPos, DirectionToRotation(secondPos - firstPos, 0f), val, Math.Min(Menu.minLength, val), Menu.breakable);
		rope.AttachEntities(firstEntity, firstPos, secondEntity, secondPos, val);
		rope.ActivatePhysics();
		group = RopeCreator.ropeGroups[Menu.liGroupIndex.SelectedIndex];
		this.rope = rope;
		this.firstEntity = firstEntity;
		firstOffset = firstEntity.GetOffsetFromWorldCoords(firstPos);
		this.secondEntity = secondEntity;
		secondOffset = secondEntity.GetOffsetFromWorldCoords(secondPos);
		length = val;
	}

	private float RadiansToDegrees(float radian)
	{
		return radian * 57.29578f;
	}

	private Vector3 DirectionToRotation(Vector3 dir, float roll)
	{
		dir = Vector3.Normalize(dir);
		Vector3 result = default(Vector3);
		result.Z = 0f - RadiansToDegrees((float)Math.Atan2(dir.X, dir.Y));
		Vector3 vector = Vector3.Normalize(new Vector3(dir.Z, new Vector3(dir.X, dir.Y, 0f).Length(), 0f));
		result.X = RadiansToDegrees((float)Math.Atan2(vector.X, vector.Y));
		result.Y = roll;
		return result;
	}

	private Prop CreateAttachProp(Vector3 pos, bool freeze = true)
	{
		Prop prop = World.CreateProp("prop_ashtray_01", pos, dynamic: true, placeOnGround: false);
		prop.FreezePosition = freeze;
		prop.IsVisible = false;
		return prop;
	}

	internal void StartWind()
	{
		Function.Call(Hash._0x1461C72C889E343E, new InputArgument[1] { rope.Handle });
		winding = true;
	}

	internal void StopWind()
	{
		Function.Call(Hash._0xCB2D4AB84A19AA7C, new InputArgument[1] { rope.Handle });
		winding = false;
	}

	internal void StartUnwind()
	{
		Function.Call(Hash._0x538D1179EC1AA9A9, new InputArgument[1] { rope.Handle });
		unwinding = true;
	}

	internal void StopUnwind()
	{
		Function.Call(Hash._0xFFF3A50779EFBBB3, new InputArgument[1] { rope.Handle });
		unwinding = false;
	}

	internal void Reattach()
	{
		if (!(rope == null) && rope.Exists() && !(firstEntity == null) && firstEntity.Exists() && !(secondEntity == null) && secondEntity.Exists())
		{
			Vector3 offsetInWorldCoords = firstEntity.GetOffsetInWorldCoords(firstOffset);
			Vector3 offsetInWorldCoords2 = secondEntity.GetOffsetInWorldCoords(secondOffset);
			rope.AttachEntities(firstEntity, offsetInWorldCoords, secondEntity, offsetInWorldCoords2, length);
			rope.ActivatePhysics();
		}
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
			if (firstEntity != null && firstEntity.Exists())
			{
				firstEntity.Delete();
			}
			firstEntity = null;
		}
		if (deleteSecondEntity)
		{
			if (firstEntity != null && firstEntity.Exists())
			{
				secondEntity.Delete();
			}
			secondEntity = null;
		}
	}
}
