using System;
using Game.Gameplay.Common;
using Game.Gameplay.Movement;
using Game.Gameplay.Perception;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.AI.Systems
{
	public sealed class AiAimSystem : ISystem
	{
		private const float MinSqrMagnitude = 0.0001f;

		private Filter _filter;

		private Stash<AimRay> _aimStash;
		private Stash<Position> _positionStash;
		private Stash<PerceptionParameters> _perceptionStash;
		private Stash<PerceivedTarget> _targetStash;
		private Stash<FacingDirection> _facingStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<AiBrain>()
				.With<AimRay>()
				.With<Position>()
				.With<PerceptionParameters>()
				.With<PerceivedTarget>()
				.With<FacingDirection>()
				.Build();

			_aimStash = World.GetStash<AimRay>();
			_positionStash = World.GetStash<Position>();
			_perceptionStash = World.GetStash<PerceptionParameters>();
			_targetStash = World.GetStash<PerceivedTarget>();
			_facingStash = World.GetStash<FacingDirection>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				ref var aim = ref _aimStash.Get(entity);
				ref var perception = ref _perceptionStash.Get(entity);
				ref var target = ref _targetStash.Get(entity);

				aim.Origin = _positionStash.Get(entity).Value + Vector3.up * perception.EyeHeight;
				aim.Direction = _facingStash.Get(entity).Value;

				if (!target.Visible || World.IsDisposed(target.Target) || !_positionStash.Has(target.Target))
					continue;

				var targetPoint = _positionStash.Get(target.Target).Value + Vector3.up * perception.TargetHeight;
				var direction = targetPoint - aim.Origin;

				if (direction.sqrMagnitude > MinSqrMagnitude)
					aim.Direction = direction.normalized;
			}
		}

		void IDisposable.Dispose() { }
	}
}
