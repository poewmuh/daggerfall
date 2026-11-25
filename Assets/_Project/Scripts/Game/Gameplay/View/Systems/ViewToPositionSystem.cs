using System;
using Game.Gameplay.Common;
using Scellecs.Morpeh;

namespace Game.Gameplay.View.Systems
{
	public sealed class ViewToPositionSystem : ISystem
	{
		private Filter _filter;
		private Stash<View> _viewStash;
		private Stash<Position> _positionStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<View>()
				.With<Position>()
				.Build();

			_viewStash = World.GetStash<View>();
			_positionStash = World.GetStash<Position>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				var view = _viewStash.Get(entity).Value;

				if (view)
					_positionStash.Get(entity).Value = view.transform.position;
			}
		}

		void IDisposable.Dispose() { }
	}
}
