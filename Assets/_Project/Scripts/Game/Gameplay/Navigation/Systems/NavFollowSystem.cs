using System;
using Game.Gameplay.Common;
using Game.Gameplay.Movement;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Navigation.Systems
{
	public sealed class NavFollowSystem : ISystem
	{
		private Filter _filter;

		private Stash<NavAgent> _agentStash;
		private Stash<NavPath> _pathStash;
		private Stash<NavDestination> _destinationStash;
		private Stash<NavArrived> _arrivedStash;
		private Stash<Position> _positionStash;
		private Stash<MoveDirection> _moveDirectionStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<NavAgent>()
				.With<NavPath>()
				.With<NavDestination>()
				.With<Position>()
				.With<MoveDirection>()
				.Build();

			_agentStash = World.GetStash<NavAgent>();
			_pathStash = World.GetStash<NavPath>();
			_destinationStash = World.GetStash<NavDestination>();
			_arrivedStash = World.GetStash<NavArrived>();
			_positionStash = World.GetStash<Position>();
			_moveDirectionStash = World.GetStash<MoveDirection>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				ref var path = ref _pathStash.Get(entity);
				ref var destination = ref _destinationStash.Get(entity);
				ref var moveDirection = ref _moveDirectionStash.Get(entity);
				var position = _positionStash.Get(entity).Value;
				var reach = _agentStash.Get(entity).CornerReachDistance;

				if (HasArrived(in path, in destination, position, reach))
				{
					moveDirection.Value = Vector3.zero;
					_arrivedStash.Set(entity);
					continue;
				}

				_arrivedStash.Remove(entity);

				if (!path.Valid)
				{
					moveDirection.Value = Vector3.zero;
					continue;
				}

				while (path.Index < path.Count - 1 && PlanarMath.Distance(position, path.Corners[path.Index]) <= reach)
				{
					path.Index++;
				}

				moveDirection.Value = PlanarMath.Flatten(path.Corners[path.Index] - position).normalized;
			}
		}

		private static bool HasArrived(in NavPath path, in NavDestination destination, Vector3 position, float reach)
		{
			if (PlanarMath.Distance(position, destination.Value) <= destination.StoppingDistance)
				return true;

			return path.Valid
				&& path.Index >= path.Count - 1
				&& PlanarMath.Distance(position, path.Corners[path.Count - 1]) <= Mathf.Max(reach, destination.StoppingDistance);
		}

		void IDisposable.Dispose() { }
	}
}
