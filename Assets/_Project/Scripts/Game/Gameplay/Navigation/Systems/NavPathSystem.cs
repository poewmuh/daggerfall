using System;
using Game.Gameplay.Common;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Gameplay.Navigation.Systems
{
	public sealed class NavPathSystem : ISystem
	{
		private const float DestinationChangeThreshold = 0.3f;

		private readonly NavMeshPath _path = new();

		private Filter _filter;

		private Stash<NavAgent> _agentStash;
		private Stash<NavPath> _pathStash;
		private Stash<NavDestination> _destinationStash;
		private Stash<Position> _positionStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<NavAgent>()
				.With<NavPath>()
				.With<NavDestination>()
				.With<Position>()
				.Build();

			_agentStash = World.GetStash<NavAgent>();
			_pathStash = World.GetStash<NavPath>();
			_destinationStash = World.GetStash<NavDestination>();
			_positionStash = World.GetStash<Position>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				ref var agent = ref _agentStash.Get(entity);
				ref var path = ref _pathStash.Get(entity);
				var destination = _destinationStash.Get(entity).Value;

				agent.RepathTimer -= deltaTime;

				var destinationChanged = !path.Valid || Vector3.Distance(path.Destination, destination) > DestinationChangeThreshold;

				if (!destinationChanged && agent.RepathTimer > 0f)
					continue;

				agent.RepathTimer = agent.RepathInterval;
				Rebuild(ref path, _positionStash.Get(entity).Value, destination, agent.SampleDistance);
			}
		}

		private void Rebuild(ref NavPath path, Vector3 from, Vector3 to, float sampleDistance)
		{
			path.Destination = to;
			path.Valid = false;
			path.Count = 0;
			path.Index = 0;

			if (!NavMesh.SamplePosition(from, out var fromHit, sampleDistance, NavMesh.AllAreas))
				return;

			if (!NavMesh.SamplePosition(to, out var toHit, sampleDistance, NavMesh.AllAreas))
				return;

			if (!NavMesh.CalculatePath(fromHit.position, toHit.position, NavMesh.AllAreas, _path) || _path.status == NavMeshPathStatus.PathInvalid)
				return;

			path.Count = _path.GetCornersNonAlloc(path.Corners);
			path.Index = path.Count > 1 ? 1 : 0;
			path.Valid = path.Count > 0;
		}

		void IDisposable.Dispose() { }
	}
}
