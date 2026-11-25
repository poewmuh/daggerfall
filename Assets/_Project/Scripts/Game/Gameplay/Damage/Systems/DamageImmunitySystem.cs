using System;
using Scellecs.Morpeh;

namespace Game.Gameplay.Damage.Systems
{
	public sealed class DamageImmunitySystem : ISystem
	{
		private Filter _filter;
		private Stash<DamageEvent> _eventStash;
		private Stash<DamageImmunity> _immunityStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<DamageEvent>()
				.Build();

			_eventStash = World.GetStash<DamageEvent>();
			_immunityStash = World.GetStash<DamageImmunity>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var damageEntity in _filter)
			{
				var target = _eventStash.Get(damageEntity).Target;

				if (World.IsDisposed(target) || !_immunityStash.Has(target))
					continue;

				if (_immunityStash.Get(target).Value != DamageImmunityReason.None)
				{
					World.RemoveEntity(damageEntity);
				}
			}
		}

		void IDisposable.Dispose() { }
	}
}
