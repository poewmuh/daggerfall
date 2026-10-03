using Game.Application.Asset;
using Game.Gameplay.View.Systems;
using Scellecs.Morpeh;

namespace Game.Gameplay.View
{
	public static class ViewFeature
	{
		public static SystemsGroup Create(World world, IAssetLoader assetLoader)
		{
			var group = world.CreateSystemsGroup();

			group.AddSystem(new CreateViewSystem(assetLoader));
			group.AddSystem(new DestroyViewSystem());

			return group;
		}
	}
}