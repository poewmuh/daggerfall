using System;
using Scellecs.Morpeh;

namespace Game.Gameplay.Movement.Systems
{
	public class MoveDirectionToVelocitySystem : IFixedSystem
	{
		private Filter _moversFilter;
		private Stash<MoveDirection> _moveDirectionStash;
		private Stash<MoveSpeed> _moveSpeedStash;
		private Stash<Velocity> _velocityStash;
		
		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_moversFilter = World.Filter
				.With<MoveDirection>()
				.With<MoveSpeed>()
				.With<Velocity>()
				.With<CanMove>()
				.Build();

			_moveDirectionStash = World.GetStash<MoveDirection>();
			_moveSpeedStash = World.GetStash<MoveSpeed>();
			_velocityStash = World.GetStash<Velocity>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var moveEntity in _moversFilter)
			{
				ref var moveInput = ref _moveDirectionStash.Get(moveEntity);
				ref var moveSpeed = ref _moveSpeedStash.Get(moveEntity);
				ref var velocity = ref _velocityStash.Get(moveEntity);
				
				velocity.Value += moveSpeed.Value * moveInput.Value;
			}
		}
		
		void IDisposable.Dispose()
		{
		}
	}
}