using System;
using Game.Gameplay.Agent;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Damage
{
	[Serializable]
	public sealed class HealthTrait : IAgentTrait
	{
		[SerializeField, Min(1f)] private float _maxHealth = 100f;

		public void Install(Entity entity, World world)
		{
			world.GetStash<Health>().Set(entity, new Health { Current = _maxHealth, Max = _maxHealth });
			world.GetStash<DamageImmunity>().Set(entity);
		}
	}
}
