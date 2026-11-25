using System;
using Game.Gameplay.Movement;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Navigation.Systems
{
	public sealed class NavIdleSystem : ISystem
	{
		private Filter _filter;

		private Stash<NavPath> _pathStash;
		private Stash<NavArrived> _arrivedStash;
		private Stash<MoveDirection> _moveDirectionStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<NavAgent>()
				.With<NavPath>()
				.With<MoveDirection>()
				.Without<NavDestination>()
				.Build();

			_pathStash = World.GetStash<NavPath>();
			_arrivedStash = World.GetStash<NavArrived>();
			_moveDirectionStash = World.GetStash<MoveDirection>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				_moveDirectionStash.Get(entity).Value = Vector3.zero;
				_pathStash.Get(entity).Valid = false;
				_arrivedStash.Remove(entity);
			}
		}

		void IDisposable.Dispose() { }
	}
}
