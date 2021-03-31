using GTA;
using System.Collections.Generic;

namespace RopeCreator
{
	class RopeGroup
	{
		internal List<AttachedRope> ropes = new List<AttachedRope>();

		internal void StartWindRopes()
		{
			if (ropes.Count == 0) return;

			foreach (var rope in ropes)
			{
				rope.StopUnwind();
				rope.StartWind();
			}
		}

		internal void StopWindRopes()
		{
			if (ropes.Count == 0) return;

			foreach (var rope in ropes)
			{
				rope.StopWind();
			}
		}

		internal void StartUnwindRopes()
		{
			if (ropes.Count == 0) return;

			foreach (var rope in ropes)
			{
				rope.StopWind();
				rope.StartUnwind();
			}
		}

		internal void StopUnwindRopes()
		{
			if (ropes.Count == 0) return;

			foreach (var rope in ropes)
			{
				rope.StopUnwind();
			}
		}

		internal void DeleteRopes()
		{
			if (ropes.Count == 0) return;

			foreach (var rope in ropes)
			{
				rope.Delete();
			}

			ropes.Clear();
		}

		internal bool AreAllRopesWinding()
		{
			if (ropes.Count == 0) return false;

			foreach (var rope in ropes)
			{
				if (!rope.winding) return false;
			}

			return true;
		}

		internal bool AreAllRopesUnwinding()
		{
			if (ropes.Count == 0) return false;

			foreach (var rope in ropes)
			{
				if (!rope.unwinding) return false;
			}

			return true;
		}

		internal void DeleteRopesWithBadEntity()
		{
			if (ropes.Count == 0) return;

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
			}
		}

		internal void ReattachRagdollPeds()
		{
			if (ropes.Count == 0) return;

			foreach (var rope in ropes)
			{
				if ((rope.firstEntity.Model.IsPed && ((Ped)rope.firstEntity).IsRagdoll) ||
					(rope.secondEntity.Model.IsPed && ((Ped)rope.secondEntity).IsRagdoll))
				{
					rope.Reattach();
				}
			}
		}
	}
}
