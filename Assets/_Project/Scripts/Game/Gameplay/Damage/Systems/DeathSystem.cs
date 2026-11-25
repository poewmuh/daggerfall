using System;
using Game.Gameplay.Actions;
using Game.Gameplay.Movement;
using Scellecs.Morpeh;

namespace Game.Gameplay.Damage.Systems
{
	public sealed class DeathSystem : ISystem
	{
		private Filter _filter;

		private Stash<Health> _healthStash;
		private Stash<Dead> _deadStash;
		private Stash<Died> _diedStash;
		private Stash<MovementBlockers> _movementBlockersStash;
		private Stash<ActionBlockers> _actionBlockersStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<Health>()
				.Without<Dead>()
				.Build();

			_healthStash = World.GetStash<Health>();
			_deadStash = World.GetStash<Dead>();
			_diedStash = World.GetStash<Died>();
			_movementBlockersStash = World.GetStash<MovementBlockers>();
			_actionBlockersStash = World.GetStash<ActionBlockers>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				if (_healthStash.Get(entity).Current > 0f)
					continue;

				_deadStash.Set(entity);
				_diedStash.Set(entity);

				if (_movementBlockersStash.Has(entity))
					_movementBlockersStash.Get(entity).Value |= MovementBlockReason.Dead;

				if (_actionBlockersStash.Has(entity))
					_actionBlockersStash.Get(entity).Value |= ActionBlockReason.Dead;
			}
		}

		void IDisposable.Dispose() { }
	}
}
