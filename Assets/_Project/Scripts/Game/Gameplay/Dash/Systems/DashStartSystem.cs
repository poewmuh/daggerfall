using System;
using Game.Gameplay.Movement;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Dash.Systems
{
	public sealed class DashStartSystem : IFixedSystem
	{
		private const float MinInputSqrMagnitude = 0.0001f;

		private Filter _filter;

		private Stash<DashParameters> _parametersStash;
		private Stash<MoveDirection> _moveDirectionStash;
		private Stash<FacingDirection> _facingStash;
		private Stash<MovementBlockers> _blockersStash;
		private Stash<Dashing> _dashingStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<DashRequest>()
				.With<DashParameters>()
				.With<MoveDirection>()
				.With<FacingDirection>()
				.With<MovementBlockers>()
				.Without<Dashing>()
				.Without<DashCooldown>()
				.Build();

			_parametersStash = World.GetStash<DashParameters>();
			_moveDirectionStash = World.GetStash<MoveDirection>();
			_facingStash = World.GetStash<FacingDirection>();
			_blockersStash = World.GetStash<MovementBlockers>();
			_dashingStash = World.GetStash<Dashing>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				ref var parameters = ref _parametersStash.Get(entity);

				var direction = ResolveDirection(
					_moveDirectionStash.Get(entity).Value,
					_facingStash.Get(entity).Value,
					parameters.FallbackDirection);

				_dashingStash.Set(entity, new Dashing
				{
					Velocity = direction * (parameters.Distance / parameters.Duration),
					TimeRemaining = parameters.Duration,
				});

				_blockersStash.Get(entity).Value |= MovementBlockReason.Dash;
			}
		}

		private static Vector3 ResolveDirection(Vector3 moveDirection, Vector3 facing, DashFallbackDirection fallback)
		{
			var planarMove = Vector3.ProjectOnPlane(moveDirection, Vector3.up);

			if (planarMove.sqrMagnitude > MinInputSqrMagnitude)
				return planarMove.normalized;

			var planarFacing = Vector3.ProjectOnPlane(facing, Vector3.up).normalized;

			return fallback switch
			{
				DashFallbackDirection.Backward => -planarFacing,
				_ => planarFacing,
			};
		}

		void IDisposable.Dispose() { }
	}
}
