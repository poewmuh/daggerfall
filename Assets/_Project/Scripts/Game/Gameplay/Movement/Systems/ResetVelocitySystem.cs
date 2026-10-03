using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Movement.Systems
{
	public class ResetVelocitySystem : IFixedSystem
	{
		public World World { get; set; }
		
		private Filter _filter;
		private Stash<Velocity> _velocityStash;
		
		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<Velocity>()
				.Build();

			_velocityStash = World.GetStash<Velocity>();
		}
		
		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				ref var velocity = ref _velocityStash.Get(entity);
				velocity.Value = Vector3.zero;
			}
		}

		void IDisposable.Dispose() { }
	}
}