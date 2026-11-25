using Scellecs.Morpeh;

namespace Game.Gameplay.Common
{
	public static class GameplayStashExtensions
	{
		public static void EnsureComponent<T>(this Stash<T> stash, Entity entity) where T : struct, IComponent
		{
			if (!stash.Has(entity))
				stash.Set(entity);
		}
	}
}
