using Game.Gameplay.Movement.Systems;
using Scellecs.Morpeh;

namespace Game.Gameplay.Movement
{
	public static class VelocityFeature
	{
		public static SystemsGroup CreateReset(World world)
		{
			var group = world.CreateSystemsGroup();

			group.AddSystem(new ResetVelocitySystem());

			return group;
		}

		public static SystemsGroup CreateApply(World world)
		{
			var group = world.CreateSystemsGroup();

			group.AddSystem(new VelocityToRigidbodySystem());

			return group;
		}
	}
}
