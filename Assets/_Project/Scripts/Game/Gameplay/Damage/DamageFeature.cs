using Game.Gameplay.Damage.Systems;
using Scellecs.Morpeh;

namespace Game.Gameplay.Damage
{
	public static class DamageFeature
	{
		public static SystemsGroup Create(World world)
		{
			var group = world.CreateSystemsGroup();

			group.AddSystem(new DamageEventCleanupSystem());

			return group;
		}
	}
}
