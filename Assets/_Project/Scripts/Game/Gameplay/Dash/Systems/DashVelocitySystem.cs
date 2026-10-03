using System;
using Game.Gameplay.Movement;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Dash.Systems
{
	public sealed class DashVelocitySystem : IFixedSystem
	{
		private Filter _filter;
		private Stash<Dashing> _dashingStash;
		private Stash<Velocity> _velocityStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<Dashing>()
				.With<Velocity>()
				.Build();

			_dashingStash = World.GetStash<Dashing>();
			_velocityStash = World.GetStash<Velocity>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				ref var dashing = ref _dashingStash.Get(entity);
				var activeFraction = Mathf.Clamp01(dashing.TimeRemaining / deltaTime);

				_velocityStash.Get(entity).Value += dashing.Velocity * activeFraction;
			}
		}

		void IDisposable.Dispose() { }
	}
}
