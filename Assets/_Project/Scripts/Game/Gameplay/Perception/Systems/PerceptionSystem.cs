using System;
using Game.Gameplay.Common;
using Game.Gameplay.Damage;
using Game.Gameplay.Input;
using Game.Gameplay.Movement;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Perception.Systems
{
	public sealed class PerceptionSystem : ISystem
	{
		private Filter _perceivers;
		private Filter _candidates;

		private Stash<PerceptionParameters> _perceptionStash;
		private Stash<PerceivedTarget> _targetStash;
		private Stash<Position> _positionStash;
		private Stash<FacingDirection> _facingStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_perceivers = World.Filter
				.With<PerceptionParameters>()
				.With<PerceivedTarget>()
				.With<Position>()
				.With<FacingDirection>()
				.Without<Dead>()
				.Build();

			_candidates = World.Filter
				.With<PlayerControlled>()
				.With<Position>()
				.Without<Dead>()
				.Build();

			_perceptionStash = World.GetStash<PerceptionParameters>();
			_targetStash = World.GetStash<PerceivedTarget>();
			_positionStash = World.GetStash<Position>();
			_facingStash = World.GetStash<FacingDirection>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var perceiver in _perceivers)
			{
				ref var perception = ref _perceptionStash.Get(perceiver);
				ref var target = ref _targetStash.Get(perceiver);

				UpdateMemory(ref target, perception.MemoryDuration, deltaTime);

				perception.Timer -= deltaTime;
				if (perception.Timer > 0f)
					continue;

				perception.Timer += perception.Interval;

				if (TryFindVisible(perceiver, in perception, out var seen, out var seenPosition))
				{
					target.Target = seen;
					target.LastKnownPosition = seenPosition;
					target.TimeSinceSeen = 0f;
					target.Visible = true;
					target.HasTarget = true;
				}
				else
				{
					target.Visible = false;
				}
			}
		}

		private static void UpdateMemory(ref PerceivedTarget target, float memoryDuration, float deltaTime)
		{
			if (!target.HasTarget || target.Visible)
				return;

			target.TimeSinceSeen += deltaTime;

			if (target.TimeSinceSeen > memoryDuration)
				target.HasTarget = false;
		}

		private bool TryFindVisible(Entity perceiver, in PerceptionParameters perception, out Entity seen, out Vector3 seenPosition)
		{
			seen = default;
			seenPosition = default;

			var origin = _positionStash.Get(perceiver).Value;
			var eye = origin + Vector3.up * perception.EyeHeight;
			var facing = Vector3.ProjectOnPlane(_facingStash.Get(perceiver).Value, Vector3.up).normalized;
			var bestDistance = float.MaxValue;

			foreach (var candidate in _candidates)
			{
				if (candidate == perceiver)
					continue;

				var candidatePosition = _positionStash.Get(candidate).Value;
				var toCandidate = candidatePosition - origin;
				var distance = toCandidate.magnitude;

				if (distance > perception.Range || distance >= bestDistance)
					continue;

				if (distance > perception.ProximityRadius && !IsInsideFov(facing, toCandidate, perception.HalfFovCos))
					continue;

				var targetPoint = candidatePosition + Vector3.up * perception.TargetHeight;

				if (Physics.Linecast(eye, targetPoint, perception.ObstacleMask, QueryTriggerInteraction.Ignore))
					continue;

				bestDistance = distance;
				seen = candidate;
				seenPosition = candidatePosition;
			}

			return bestDistance < float.MaxValue;
		}

		private static bool IsInsideFov(Vector3 facing, Vector3 toCandidate, float halfFovCos)
		{
			var planar = Vector3.ProjectOnPlane(toCandidate, Vector3.up);

			if (planar.sqrMagnitude < 0.0001f)
				return true;

			return Vector3.Dot(facing, planar.normalized) >= halfFovCos;
		}

		void IDisposable.Dispose() { }
	}
}
