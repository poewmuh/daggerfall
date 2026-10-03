using Game.Gameplay.Common.Systems;
using Game.Gameplay.Dash.Systems;
using Scellecs.Morpeh;

namespace Game.Gameplay.Dash
{
	public static class DashFeature
	{
		public static SystemsGroup Create(World world)
		{
			var group = world.CreateSystemsGroup();

			group.AddSystem(new DashCooldownSystem());
			group.AddSystem(new DashStartSystem());
			group.AddSystem(new DashVelocitySystem());
			group.AddSystem(new DashEndSystem());
			group.AddSystem(new FixedComponentCleanupSystem<DashRequest>());

			return group;
		}
	}
}
