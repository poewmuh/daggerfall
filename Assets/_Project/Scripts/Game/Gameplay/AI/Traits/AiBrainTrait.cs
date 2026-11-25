using System;
using Game.Gameplay.Agent;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.AI.Traits
{
	[Serializable]
	public sealed class AiBrainTrait : IAgentTrait
	{
		[SerializeField, Range(0f, 1f)] private float _inertia = 0.1f;

		public void Install(Entity entity, World world)
		{
			world.GetStash<AiBrain>().Set(entity, new AiBrain { Inertia = _inertia });
			world.GetStash<AiContext>().Set(entity, new AiContext { HealthRatio = 1f });
		}
	}
}
