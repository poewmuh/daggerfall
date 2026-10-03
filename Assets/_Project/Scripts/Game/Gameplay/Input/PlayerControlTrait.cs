using System;
using Game.Gameplay.Agent;
using Scellecs.Morpeh;

namespace Game.Gameplay.Input
{
	[Serializable]
	public sealed class PlayerControlTrait : IAgentTrait
	{
		public void Install(Entity entity, World world)
		{
			world.GetStash<PlayerControlled>().Set(entity);
			world.GetStash<InputMoveDirection>().Set(entity);
		}
	}
}
