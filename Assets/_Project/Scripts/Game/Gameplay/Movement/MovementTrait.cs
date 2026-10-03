using System;
using Game.Gameplay.Agent;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Movement
{
	[Serializable]
	public sealed class MovementTrait : IAgentTrait
	{
		[SerializeField, Min(0f)] private float _moveSpeed = 5f;

		public void Install(Entity entity, World world)
		{
			world.GetStash<MoveSpeed>().Set(entity, new MoveSpeed { Value = _moveSpeed });
			world.GetStash<MovementBlockers>().Set(entity);
			world.GetStash<Velocity>().Set(entity);
			world.GetStash<MoveDirection>().Set(entity);
			world.GetStash<FacingDirection>().Set(entity, new FacingDirection { Value = Vector3.forward });
		}
	}
}
