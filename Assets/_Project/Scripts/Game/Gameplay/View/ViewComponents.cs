using Scellecs.Morpeh;

namespace Game.Gameplay.View
{
	public struct View : IComponent
	{
		public EntityInitializer Value;
	}

	public struct AssetPath : IComponent
	{
		public string Value;
	}
}