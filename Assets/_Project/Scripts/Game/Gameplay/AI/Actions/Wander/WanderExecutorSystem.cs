using System;
using Game.Gameplay.Navigation;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Gameplay.AI.Actions.Wander
{
	public sealed class WanderExecutorSystem : ISystem
	{
		private Filter _filter;

		private Stash<AiAction> _actionStash;
		private Stash<AiActivated> _activatedStash;
		private Stash<WanderAction> _wanderStash;
		private Stash<NavDestination> _destinationStash;
		private Stash<NavArrived> _arrivedStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<AiAction>()
				.With<AiActive>()
				.With<WanderAction>()
				.Build();

			_actionStash = World.GetStash<AiAction>();
			_activatedStash = World.GetStash<AiActivated>();
			_wanderStash = World.GetStash<WanderAction>();
			_destinationStash = World.GetStash<NavDestination>();
			_arrivedStash = World.GetStash<NavArrived>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var action in _filter)
			{
				var owner = _actionStash.Get(action).Owner;

				if (World.IsDisposed(owner))
					continue;

				ref var wander = ref _wanderStash.Get(action);

				if (_activatedStash.Has(action))
				{
					wander.Waiting = true;
					wander.Timer = UnityEngine.Random.Range(0f, wander.WaitMin);
				}

				wander.Timer -= deltaTime;

				if (wander.Waiting)
				{
					if (wander.Timer <= 0f)
						BeginTravel(owner, ref wander);

					continue;
				}

				if (_arrivedStash.Has(owner) || wander.Timer <= 0f)
					BeginWait(owner, ref wander);
			}
		}

		private void BeginTravel(Entity owner, ref WanderAction wander)
		{
			var offset = UnityEngine.Random.insideUnitCircle * wander.Radius;
			var candidate = wander.Home + new Vector3(offset.x, 0f, offset.y);

			if (!NavMesh.SamplePosition(candidate, out var hit, wander.Radius, NavMesh.AllAreas))
			{
				wander.Timer = wander.WaitMin;
				return;
			}

			_destinationStash.Set(owner, new NavDestination
			{
				Value = hit.position,
				StoppingDistance = wander.StoppingDistance,
			});

			wander.Waiting = false;
			wander.Timer = wander.MaxTravelTime;
		}

		private void BeginWait(Entity owner, ref WanderAction wander)
		{
			_destinationStash.Remove(owner);
			wander.Waiting = true;
			wander.Timer = UnityEngine.Random.Range(wander.WaitMin, wander.WaitMax);
		}

		void IDisposable.Dispose() { }
	}
}
