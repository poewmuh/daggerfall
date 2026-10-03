using System;
using Scellecs.Morpeh;

namespace Game.Gameplay.Damage.Systems
{
	public sealed class DamageEventCleanupSystem : ICleanupSystem
	{
		private Filter _filter;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<DamageEvent>()
				.Build();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				World.RemoveEntity(entity);
			}
		}

		void IDisposable.Dispose() { }
	}
}
