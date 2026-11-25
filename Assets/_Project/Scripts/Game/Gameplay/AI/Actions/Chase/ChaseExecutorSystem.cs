using System;
using Game.Gameplay.Common;
using Game.Gameplay.Movement;
using Game.Gameplay.Navigation;
using Game.Gameplay.Perception;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.AI.Actions.Chase
{
	public sealed class ChaseExecutorSystem : ISystem
	{
		private Filter _filter;

		private Stash<AiAction> _actionStash;
		private Stash<ChaseAction> _chaseStash;
		private Stash<PerceivedTarget> _targetStash;
		private Stash<Position> _positionStash;
		private Stash<NavDestination> _destinationStash;
		private Stash<NavArrived> _arrivedStash;
		private Stash<FacingDirection> _facingStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<AiAction>()
				.With<AiActive>()
				.With<ChaseAction>()
				.Build();

			_actionStash = World.GetStash<AiAction>();
			_chaseStash = World.GetStash<ChaseAction>();
			_targetStash = World.GetStash<PerceivedTarget>();
			_positionStash = World.GetStash<Position>();
			_destinationStash = World.GetStash<NavDestination>();
			_arrivedStash = World.GetStash<NavArrived>();
			_facingStash = World.GetStash<FacingDirection>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var action in _filter)
			{
				var owner = _actionStash.Get(action).Owner;

				if (World.IsDisposed(owner) || !_targetStash.Has(owner))
					continue;

				ref var target = ref _targetStash.Get(owner);

				if (!target.HasTarget)
				{
					_destinationStash.Remove(owner);
					continue;
				}

				ref var chase = ref _chaseStash.Get(action);
				var targetAlive = !World.IsDisposed(target.Target) && _positionStash.Has(target.Target);
				var tracking = target.Visible && targetAlive;
				var destination = tracking ? _positionStash.Get(target.Target).Value : target.LastKnownPosition;

				_destinationStash.Set(owner, new NavDestination
				{
					Value = destination,
					StoppingDistance = tracking ? chase.StoppingDistance : chase.LastKnownStoppingDistance,
				});

				if (tracking && _arrivedStash.Has(owner) && _facingStash.Has(owner) && _positionStash.Has(owner))
					FaceTowards(owner, destination);
			}
		}

		private void FaceTowards(Entity owner, Vector3 point)
		{
			var direction = PlanarMath.Flatten(point - _positionStash.Get(owner).Value);

			if (direction.sqrMagnitude > 0.0001f)
				_facingStash.Get(owner).Value = direction.normalized;
		}

		void IDisposable.Dispose() { }
	}
}
