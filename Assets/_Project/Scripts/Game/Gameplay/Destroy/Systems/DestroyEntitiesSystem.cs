using System;
using Scellecs.Morpeh;

namespace Game.Gameplay.Destroy.Systems
{
	public class DestroyEntitiesSystem : ICleanupSystem
	{
		public World World { get; set; }
		
		private Filter _filter;

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<DestroySelfRequest>()
				.Build();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				World.RemoveEntity(entity);
			}
		}

		void IDisposable.Dispose()
		{
			
		}
	}
}