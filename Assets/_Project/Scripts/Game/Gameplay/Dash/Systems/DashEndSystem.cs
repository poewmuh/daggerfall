using System;
using Game.Gameplay.Actions;
using Game.Gameplay.Damage;
using Game.Gameplay.Movement;
using Scellecs.Morpeh;

namespace Game.Gameplay.Dash.Systems
{
	public sealed class DashEndSystem : IFixedSystem
	{
		private Filter _filter;

		private Stash<Dashing> _dashingStash;
		private Stash<DashParameters> _parametersStash;
		private Stash<DashCooldown> _cooldownStash;
		private Stash<MovementBlockers> _blockersStash;
		private Stash<ActionBlockers> _actionBlockersStash;
		private Stash<DamageImmunity> _immunityStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<Dashing>()
				.With<DashParameters>()
				.With<MovementBlockers>()
				.Build();

			_dashingStash = World.GetStash<Dashing>();
			_parametersStash = World.GetStash<DashParameters>();
			_cooldownStash = World.GetStash<DashCooldown>();
			_blockersStash = World.GetStash<MovementBlockers>();
			_actionBlockersStash = World.GetStash<ActionBlockers>();
			_immunityStash = World.GetStash<DamageImmunity>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				ref var dashing = ref _dashingStash.Get(entity);
				dashing.TimeRemaining -= deltaTime;

				if (dashing.TimeRemaining > 0f && !IsInterrupted(entity))
					continue;

				_dashingStash.Remove(entity);
				_blockersStash.Get(entity).Value &= ~MovementBlockReason.Dash;

				if (_immunityStash.Has(entity))
					_immunityStash.Get(entity).Value &= ~DamageImmunityReason.Dash;

				var cooldown = _parametersStash.Get(entity).Cooldown;

				if (cooldown > 0f)
				{
					_cooldownStash.Set(entity, new DashCooldown { Remaining = cooldown });
				}
			}
		}

		private bool IsInterrupted(Entity entity)
		{
			return _actionBlockersStash.Has(entity) && _actionBlockersStash.Get(entity).Value != ActionBlockReason.None;
		}

		void IDisposable.Dispose() { }
	}
}
