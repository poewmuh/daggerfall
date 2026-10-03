using System;
using Game.Application.Asset;
using Game.Gameplay.Common;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.View.Systems
{
	public class CreateViewSystem : ISystem
	{
		public World World { get; set; }

		private Filter _filter;

		private Stash<AssetPath> _assetPathStash;
		private Stash<Position> _positionStash;
		private Stash<View> _viewStash;
		
		private readonly IAssetLoader _assetLoader;

		public CreateViewSystem(IAssetLoader assetLoader)
		{
			_assetLoader = assetLoader;
		}
		
		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<AssetPath>()
				.With<Position>()
				.Without<View>()
				.Build();
			
			_assetPathStash = World.GetStash<AssetPath>();
			_positionStash = World.GetStash<Position>();
			_viewStash = World.GetStash<View>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				ref var assetPath = ref _assetPathStash.Get(entity);
				ref var position = ref _positionStash.Get(entity);

				var prefab = _assetLoader.LoadImmediate<GameObject>(assetPath.Value);
				var viewObject = UnityEngine.Object.Instantiate(prefab, position.Value, Quaternion.identity, null);
				var view = viewObject.GetComponent<EntityInitializer>();
				view.Bind(entity, World);
				_viewStash.Add(entity, new View { Value = view });
			}
		}

		void IDisposable.Dispose()
		{
			
		}
	}
}