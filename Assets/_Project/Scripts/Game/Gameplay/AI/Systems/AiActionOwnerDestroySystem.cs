using System;
using Game.Gameplay.Destroy;
using Scellecs.Morpeh;

namespace Game.Gameplay.AI.Systems
{
	public sealed class AiActionOwnerDestroySystem : ICleanupSystem
	{
		private Filter _filter;

		private Stash<AiAction> _actionStash;
		private Stash<DestroySelfRequest> _destroyStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<AiAction>()
				.Without<DestroySelfRequest>()
				.Build();

			_actionStash = World.GetStash<AiAction>();
			_destroyStash = World.GetStash<DestroySelfRequest>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var action in _filter)
			{
				var owner = _actionStash.Get(action).Owner;

				if (World.IsDisposed(owner) || _destroyStash.Has(owner))
					_destroyStash.Set(action);
			}
		}

		void IDisposable.Dispose() { }
	}
}
