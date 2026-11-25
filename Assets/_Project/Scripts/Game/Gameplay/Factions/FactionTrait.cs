using System;
using Game.Gameplay.Agent;
using Scellecs.Morpeh;
using TriInspector;
using UnityEngine;

namespace Game.Gameplay.Factions
{
	[Serializable]
	public sealed class FactionTrait : IAgentTrait
	{
		[Required]
		[SerializeField] private FactionDefinition _faction;

		public void Install(Entity entity, World world)
		{
			world.GetStash<Faction>().Set(entity, new Faction { Value = _faction });
		}
	}
}
