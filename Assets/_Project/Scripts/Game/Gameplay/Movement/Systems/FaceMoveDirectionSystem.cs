using System;
using Game.Gameplay.Input;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Movement.Systems
{
	public sealed class FaceMoveDirectionSystem : ISystem
	{
		private const float MinSqrMagnitude = 0.0001f;

		private Filter _filter;
		private Stash<MoveDirection> _moveDirectionStash;
		private Stash<FacingDirection> _facingStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<MoveDirection>()
				.With<FacingDirection>()
				.Without<PlayerControlled>()
				.Build();

			_moveDirectionStash = World.GetStash<MoveDirection>();
			_facingStash = World.GetStash<FacingDirection>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				var direction = Vector3.ProjectOnPlane(_moveDirectionStash.Get(entity).Value, Vector3.up);

				if (direction.sqrMagnitude > MinSqrMagnitude)
					_facingStash.Get(entity).Value = direction.normalized;
			}
		}

		void IDisposable.Dispose() { }
	}
}
