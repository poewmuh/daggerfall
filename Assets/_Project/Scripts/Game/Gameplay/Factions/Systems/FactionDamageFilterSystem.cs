using System;
using Game.Gameplay.Damage;
using Scellecs.Morpeh;

namespace Game.Gameplay.Factions.Systems
{
	public sealed class FactionDamageFilterSystem : ISystem
	{
		private Filter _filter;
		private Stash<DamageEvent> _eventStash;
		private Stash<Faction> _factionStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<DamageEvent>()
				.Build();

			_eventStash = World.GetStash<DamageEvent>();
			_factionStash = World.GetStash<Faction>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var damageEntity in _filter)
			{
				ref var damage = ref _eventStash.Get(damageEntity);

				if (World.IsDisposed(damage.Source) || World.IsDisposed(damage.Target))
					continue;

				if (_factionStash.HasFactions(damage.Source, damage.Target) && !_factionStash.AreHostile(damage.Source, damage.Target))
					World.RemoveEntity(damageEntity);
			}
		}

		void IDisposable.Dispose() { }
	}
}
