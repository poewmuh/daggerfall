using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Damage.Systems
{
	public sealed class DamageApplySystem : ISystem
	{
		private Filter _filter;
		private Stash<DamageEvent> _eventStash;
		private Stash<Health> _healthStash;
		private Stash<Dead> _deadStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<DamageEvent>()
				.Build();

			_eventStash = World.GetStash<DamageEvent>();
			_healthStash = World.GetStash<Health>();
			_deadStash = World.GetStash<Dead>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var damageEntity in _filter)
			{
				ref var damage = ref _eventStash.Get(damageEntity);
				var target = damage.Target;

				if (World.IsDisposed(target) || !_healthStash.Has(target) || _deadStash.Has(target))
					continue;

				ref var health = ref _healthStash.Get(target);
				health.Current = Mathf.Max(0f, health.Current - damage.Amount);
			}
		}

		void IDisposable.Dispose() { }
	}
}
