using Game.Application.Asset;
using Game.Gameplay.View.Systems;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.View
{
	public static class ViewFeature
	{
		public static SystemsGroup Create(World world, IAssetLoader assetLoader, Camera camera)
		{
			var group = world.CreateSystemsGroup();

			group.AddSystem(new CreateViewSystem(assetLoader));
			group.AddSystem(new ViewToPositionSystem());
			group.AddSystem(new BillboardSystem(camera));
			group.AddSystem(new DestroyViewSystem());

			return group;
		}
	}
}
