using System;
using Game.Gameplay.Input;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Movement.Systems
{
	public class CameraRelativeInputToMoveDirectionSystem : ISystem
	{
		private Filter _filter;
		
		private Stash<InputMoveDirection> _inputStash;
		private Stash<MoveDirection> _directionStash;

		private readonly Camera _camera;
		
		public World World { get; set; }

		public CameraRelativeInputToMoveDirectionSystem(Camera camera)
		{
			_camera = camera;
		}
		
		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<InputMoveDirection>()
				.With<MoveDirection>()
				.Build();

			_inputStash = World.GetStash<InputMoveDirection>();
			_directionStash = World.GetStash<MoveDirection>();
		}
		
		void ISystem.OnUpdate(float deltaTime)
		{
			var basis = CameraPlanarBasis.From(_camera);
			
			foreach (var entity in _filter)
			{
				ref var input = ref _inputStash.Get(entity);
				ref var direction = ref _directionStash.Get(entity);
				
				var clampedInput = Vector2.ClampMagnitude(input.Value, 1f);
				direction.Value = basis.Right * clampedInput.x + basis.Forward * clampedInput.y;
			}
		}

		void IDisposable.Dispose() { }
	}
}
