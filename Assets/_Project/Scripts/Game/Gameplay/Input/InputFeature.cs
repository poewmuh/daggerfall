using Game.Application.InputSystem;
using Game.Gameplay.Input.Systems;
using Scellecs.Morpeh;

namespace Game.Gameplay.Input
{
	public static class InputFeature
	{
		public static SystemsGroup Create(World world, IInputSystem inputSystem)
		{
			var group = world.CreateSystemsGroup();

			group.AddInitializer(new MoveInputApplyInitializer(inputSystem));
			group.AddInitializer(new DashInputApplyInitializer(inputSystem));
			group.AddInitializer(new AttackInputApplyInitializer(inputSystem));

			return group;
		}
	}
}
