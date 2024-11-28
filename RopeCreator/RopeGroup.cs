using System.Collections.Generic;
using GTA;

namespace RopeCreator;

internal class RopeGroup
{
	internal List<AttachedRope> ropes = new List<AttachedRope>();

	internal void StartWindRopes()
	{
		if (ropes.Count == 0)
		{
			return;
		}
		foreach (AttachedRope rope in ropes)
		{
			rope.StopUnwind();
			rope.StartWind();
		}
	}

	internal void StopWindRopes()
	{
		if (ropes.Count == 0)
		{
			return;
		}
		foreach (AttachedRope rope in ropes)
		{
			rope.StopWind();
		}
	}

	internal void StartUnwindRopes()
	{
		if (ropes.Count == 0)
		{
			return;
		}
		foreach (AttachedRope rope in ropes)
		{
			rope.StopWind();
			rope.StartUnwind();
		}
	}

	internal void StopUnwindRopes()
	{
		if (ropes.Count == 0)
		{
			return;
		}
		foreach (AttachedRope rope in ropes)
		{
			rope.StopUnwind();
		}
	}

	internal void DeleteRopes()
	{
		if (ropes.Count == 0)
		{
			return;
		}
		foreach (AttachedRope rope in ropes)
		{
			rope.Delete();
		}
		ropes.Clear();
	}

	internal bool AreAllRopesWinding()
	{
		if (ropes.Count == 0)
		{
			return false;
		}
		foreach (AttachedRope rope in ropes)
		{
			if (!rope.winding)
			{
				return false;
			}
		}
		return true;
	}

	internal bool AreAllRopesUnwinding()
	{
		if (ropes.Count == 0)
		{
			return false;
		}
		foreach (AttachedRope rope in ropes)
		{
			if (!rope.unwinding)
			{
				return false;
			}
		}
		return true;
	}

	internal bool DeleteRopesWithBadEntity()
	{
		if (ropes.Count == 0)
		{
			return false;
		}
		bool result = false;
		for (int i = 0; i < ropes.Count; i++)
		{
			AttachedRope attachedRope = ropes[i];
			if (attachedRope.firstEntity == null || !attachedRope.firstEntity.Exists() || (attachedRope.firstEntity.Model.IsPed && attachedRope.firstEntity.IsDead) || attachedRope.secondEntity == null || !attachedRope.secondEntity.Exists() || (attachedRope.secondEntity.Model.IsPed && attachedRope.secondEntity.IsDead))
			{
				attachedRope.Delete();
				ropes.RemoveAt(i);
				i--;
				result = true;
			}
		}
		return result;
	}

	internal void ReattachRagdollPeds()
	{
		if (ropes.Count == 0)
		{
			return;
		}
		foreach (AttachedRope rope in ropes)
		{
			if ((rope.firstEntity.Model.IsPed && ((Ped)rope.firstEntity).IsRagdoll) || (rope.secondEntity.Model.IsPed && ((Ped)rope.secondEntity).IsRagdoll))
			{
				rope.Reattach();
			}
		}
	}
}
