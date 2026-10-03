using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Movement.Systems
{
	public class VelocityToRigidbodySystem : IFixedSystem
	{
		private Filter _moversFilter;
		private Stash<RigidbodyComponent> _rigidbodyStash;
		private Stash<Velocity> _velocityStash;
		
		public World World { get; set; }
		
		void IInitializer.OnAwake()
		{
			_moversFilter = World.Filter
				.With<Velocity>()
				.With<RigidbodyComponent>()
				.Build();

			_velocityStash = World.GetStash<Velocity>();
			_rigidbodyStash = World.GetStash<RigidbodyComponent>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var moveEntity in _moversFilter)
			{
				ref var rigidbody = ref _rigidbodyStash.Get(moveEntity);
				ref var velocity = ref _velocityStash.Get(moveEntity);

				rigidbody.Value.linearVelocity = new Vector3(
					velocity.Value.x, 
					rigidbody.Value.linearVelocity.y, 
					velocity.Value.z);
			}
		}
		
		void IDisposable.Dispose()
		{
			
		}
	}
}