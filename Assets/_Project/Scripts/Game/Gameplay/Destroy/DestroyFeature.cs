using Game.Gameplay.Destroy.Systems;
using Scellecs.Morpeh;

namespace Game.Gameplay.Destroy
{
	public class DestroyFeature
	{
		public static SystemsGroup Create(World world)
		{
			var group = world.CreateSystemsGroup();

			group.AddSystem(new DestroyEntitiesSystem());

			return group;
		}
	}
}