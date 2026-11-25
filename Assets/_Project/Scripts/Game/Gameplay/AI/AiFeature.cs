using Game.Gameplay.AI.Actions.Chase;
using Game.Gameplay.AI.Actions.Wander;
using Game.Gameplay.AI.Systems;
using Game.Gameplay.Common.Systems;
using Scellecs.Morpeh;

namespace Game.Gameplay.AI
{
	public static class AiFeature
	{
		public static SystemsGroup Create(World world)
		{
			var group = world.CreateSystemsGroup();

			group.AddSystem(new AiContextSystem());
			group.AddSystem(new AiScoringSystem());
			group.AddSystem(new AiSelectionSystem());
			group.AddSystem(new WanderExecutorSystem());
			group.AddSystem(new ChaseExecutorSystem());
			group.AddSystem(new ComponentCleanupSystem<AiActivated>());
			group.AddSystem(new AiActionOwnerDestroySystem());

			return group;
		}
	}
}
