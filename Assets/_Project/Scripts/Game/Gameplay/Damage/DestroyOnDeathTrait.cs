using System;
using Game.Gameplay.Agent;
using Scellecs.Morpeh;

namespace Game.Gameplay.Damage
{
	public struct DestroyOnDeath : IComponent { }

	[Serializable]
	public sealed class DestroyOnDeathTrait : IAgentTrait
	{
		public void Install(Entity entity, World world)
		{
			world.GetStash<DestroyOnDeath>().Set(entity);
		}
	}
}
