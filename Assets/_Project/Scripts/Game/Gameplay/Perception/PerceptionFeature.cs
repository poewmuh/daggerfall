using Game.Gameplay.Perception.Systems;
using Scellecs.Morpeh;

namespace Game.Gameplay.Perception
{
	public static class PerceptionFeature
	{
		public static SystemsGroup Create(World world)
		{
			var group = world.CreateSystemsGroup();

			group.AddSystem(new PerceptionSystem());

			return group;
		}
	}
}
