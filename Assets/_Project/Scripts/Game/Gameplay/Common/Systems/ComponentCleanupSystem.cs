using System;
using Scellecs.Morpeh;

namespace Game.Gameplay.Common.Systems
{
	public sealed class ComponentCleanupSystem<T> : ISystem where T : struct, IComponent
	{
		private Filter _filter;
		private Stash<T> _stash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter.With<T>().Build();
			_stash = World.GetStash<T>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				_stash.Remove(entity);
			}
		}

		void IDisposable.Dispose() { }
	}

	public sealed class FixedComponentCleanupSystem<T> : IFixedSystem where T : struct, IComponent
	{
		private Filter _filter;
		private Stash<T> _stash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter.With<T>().Build();
			_stash = World.GetStash<T>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				_stash.Remove(entity);
			}
		}

		void IDisposable.Dispose() { }
	}
}
