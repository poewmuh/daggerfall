using System;
using Scellecs.Morpeh;

namespace Game.Gameplay.Combat.Systems
{
	public sealed class AttackStartSystem : ISystem
	{
		private Filter _filter;

		private Stash<EquippedWeapon> _equippedStash;
		private Stash<AttackTiming> _timingStash;
		private Stash<ChargeAttack> _chargeStash;
		private Stash<AttackCharging> _chargingStash;
		private Stash<AttackSwing> _swingStash;
		private Stash<AttackRecovery> _recoveryStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<AttackRequest>()
				.With<EquippedWeapon>()
				.Build();

			_equippedStash = World.GetStash<EquippedWeapon>();
			_timingStash = World.GetStash<AttackTiming>();
			_chargeStash = World.GetStash<ChargeAttack>();
			_chargingStash = World.GetStash<AttackCharging>();
			_swingStash = World.GetStash<AttackSwing>();
			_recoveryStash = World.GetStash<AttackRecovery>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var owner in _filter)
			{
				var weapon = _equippedStash.Get(owner).Value;

				if (!CanStart(weapon))
					continue;

				if (_chargeStash.Has(weapon))
					_chargingStash.Set(weapon, new AttackCharging());
				else
					_swingStash.Set(weapon, new AttackSwing());
			}
		}

		private bool CanStart(Entity weapon)
		{
			return !World.IsDisposed(weapon)
				&& _timingStash.Has(weapon)
				&& !_chargingStash.Has(weapon)
				&& !_swingStash.Has(weapon)
				&& !_recoveryStash.Has(weapon);
		}

		void IDisposable.Dispose() { }
	}
}
