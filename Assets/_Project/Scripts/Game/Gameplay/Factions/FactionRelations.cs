using Scellecs.Morpeh;

namespace Game.Gameplay.Factions
{
	public static class FactionRelations
	{
		public static bool AreHostile(this Stash<Faction> factions, Entity source, Entity target)
		{
			if (!factions.Has(source) || !factions.Has(target))
				return false;

			var sourceFaction = factions.Get(source).Value;
			return sourceFaction != null && sourceFaction.IsHostileTo(factions.Get(target).Value);
		}

		public static bool HasFactions(this Stash<Faction> factions, Entity source, Entity target)
		{
			return factions.Has(source) && factions.Has(target);
		}
	}
}
