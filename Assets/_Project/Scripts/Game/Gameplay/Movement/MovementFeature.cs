using Game.Gameplay.Movement.Systems;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Movement
{
	public static class MovementFeature
	{
		public static SystemsGroup Create(World world, Camera camera)
		{
			var group = world.CreateSystemsGroup();

			group.AddSystem(new CameraRelativeInputToMoveDirectionSystem(camera));
			group.AddSystem(new CameraFacingSystem(camera));
			group.AddSystem(new MovementAvailabilitySystem());
			group.AddSystem(new MoveDirectionToVelocitySystem());
			
			return group;
		}
	}
}
