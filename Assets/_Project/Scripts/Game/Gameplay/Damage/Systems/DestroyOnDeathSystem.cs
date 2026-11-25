using System;
using Game.Gameplay.Destroy;
using Scellecs.Morpeh;

namespace Game.Gameplay.Damage.Systems
{
	public sealed class DestroyOnDeathSystem : ISystem
	{
		private Filter _filter;
		private Stash<DestroySelfRequest> _destroyStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<Died>()
				.With<DestroyOnDeath>()
				.Build();

			_destroyStash = World.GetStash<DestroySelfRequest>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				_destroyStash.Set(entity);
			}
		}

		void IDisposable.Dispose() { }
	}
}
