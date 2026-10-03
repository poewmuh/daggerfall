using System;
using Game.Gameplay.Destroy;
using Scellecs.Morpeh;

namespace Game.Gameplay.View.Systems
{
	public class DestroyViewSystem : ICleanupSystem
	{
		public World World { get; set; }
		
		private Filter _destroyFilter;
		private Filter _viewFilter;

		private Stash<View> _viewStash;
		
		void IInitializer.OnAwake()
		{
			_viewFilter = World.Filter
				.With<View>()
				.Build();

			_destroyFilter = World.Filter
				.With<View>()
				.With<DestroySelfRequest>()
				.Build();
			
			_viewStash = World.GetStash<View>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _destroyFilter)
			{
				DestroyView(entity);
			}
		}

		private void DestroyView(Entity entity)
		{
			ref var view = ref _viewStash.Get(entity);

			if (!view.Value) return;
			
			view.Value.Unbind();
			UnityEngine.Object.Destroy(view.Value);
		}

		void IDisposable.Dispose()
		{
			foreach (var entity in _viewFilter)
			{
				DestroyView(entity);
			}
		}
	}
}