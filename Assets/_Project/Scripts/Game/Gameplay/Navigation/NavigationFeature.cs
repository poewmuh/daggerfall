using Game.Gameplay.Navigation.Systems;
using Scellecs.Morpeh;

namespace Game.Gameplay.Navigation
{
	public static class NavigationFeature
	{
		public static SystemsGroup Create(World world)
		{
			var group = world.CreateSystemsGroup();

			group.AddSystem(new NavPathSystem());
			group.AddSystem(new NavFollowSystem());
			group.AddSystem(new NavIdleSystem());

			return group;
		}
	}
}
