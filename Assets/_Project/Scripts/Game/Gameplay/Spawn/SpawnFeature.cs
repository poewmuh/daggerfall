using Game.Gameplay.Agent;
using Game.Gameplay.Spawn.Systems;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Spawn
{
	public static class SpawnFeature
	{
		public static SystemsGroup Create(World world, AgentConfig playerConfig, Vector3 playerPosition)
		{
			var group = world.CreateSystemsGroup();

			group.AddInitializer(new AgentSpawnInitializer(playerConfig, playerPosition));

			return group;
		}
	}
}
