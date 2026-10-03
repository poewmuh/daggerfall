using Game.Gameplay.Common;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Agent
{
	public static class AgentFactory
	{
		public static Entity CreateAgent(this World world, AgentConfig config, Vector3 position)
		{
			var entity = world.CreateEntity();

			world.GetStash<Position>().Set(entity, new Position { Value = position });
			config.Install(entity, world);

			return entity;
		}
	}
}
