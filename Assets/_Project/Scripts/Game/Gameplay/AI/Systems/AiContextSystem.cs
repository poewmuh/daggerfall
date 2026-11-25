using System;
using Game.Gameplay.Common;
using Game.Gameplay.Damage;
using Game.Gameplay.Perception;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.AI.Systems
{
	public sealed class AiContextSystem : ISystem
	{
		private Filter _filter;

		private Stash<AiContext> _contextStash;
		private Stash<Position> _positionStash;
		private Stash<PerceptionParameters> _perceptionStash;
		private Stash<PerceivedTarget> _targetStash;
		private Stash<Health> _healthStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<AiBrain>()
				.With<AiContext>()
				.With<Position>()
				.Build();

			_contextStash = World.GetStash<AiContext>();
			_positionStash = World.GetStash<Position>();
			_perceptionStash = World.GetStash<PerceptionParameters>();
			_targetStash = World.GetStash<PerceivedTarget>();
			_healthStash = World.GetStash<Health>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				ref var context = ref _contextStash.Get(entity);
				var position = _positionStash.Get(entity).Value;

				FillPerception(entity, position, ref context);
				FillHealth(entity, ref context);
			}
		}

		private void FillPerception(Entity entity, Vector3 position, ref AiContext context)
		{
			context.HasTarget = false;
			context.TargetVisible = false;

			if (!_perceptionStash.Has(entity) || !_targetStash.Has(entity))
				return;

			ref var perception = ref _perceptionStash.Get(entity);
			ref var target = ref _targetStash.Get(entity);

			context.PerceptionRange = perception.Range;
			context.MemoryDuration = perception.MemoryDuration;
			context.HasTarget = target.HasTarget;
			context.TargetVisible = target.HasTarget && target.Visible;
			context.TimeSinceSeen = target.TimeSinceSeen;

			var targetPosition = target.Visible && !World.IsDisposed(target.Target) && _positionStash.Has(target.Target)
				? _positionStash.Get(target.Target).Value
				: target.LastKnownPosition;

			context.DistanceToTarget = Vector3.Distance(position, targetPosition);
		}

		private void FillHealth(Entity entity, ref AiContext context)
		{
			if (!_healthStash.Has(entity))
				return;

			ref var health = ref _healthStash.Get(entity);
			context.HealthRatio = health.Max > 0f ? health.Current / health.Max : 0f;
		}

		void IDisposable.Dispose() { }
	}
}
