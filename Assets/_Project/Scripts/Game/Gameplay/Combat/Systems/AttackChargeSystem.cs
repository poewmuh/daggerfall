using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Combat.Systems
{
	public sealed class AttackChargeSystem : ISystem
	{
		private Filter _filter;

		private Stash<AttackCharging> _chargingStash;
		private Stash<ChargeAttack> _chargeStash;
		private Stash<WeaponOwner> _ownerStash;
		private Stash<AttackIntent> _intentStash;
		private Stash<AttackSwing> _swingStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<AttackCharging>()
				.With<ChargeAttack>()
				.With<WeaponOwner>()
				.Build();

			_chargingStash = World.GetStash<AttackCharging>();
			_chargeStash = World.GetStash<ChargeAttack>();
			_ownerStash = World.GetStash<WeaponOwner>();
			_intentStash = World.GetStash<AttackIntent>();
			_swingStash = World.GetStash<AttackSwing>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var weapon in _filter)
			{
				ref var charging = ref _chargingStash.Get(weapon);
				charging.Elapsed += deltaTime;

				if (IsHeld(_ownerStash.Get(weapon).Value))
					continue;

				var charge = Mathf.Clamp01(charging.Elapsed / _chargeStash.Get(weapon).MaxDuration);

				_chargingStash.Remove(weapon);
				_swingStash.Set(weapon, new AttackSwing { Charge = charge });
			}
		}

		private bool IsHeld(Entity owner)
		{
			return !World.IsDisposed(owner) && _intentStash.Has(owner) && _intentStash.Get(owner).Held;
		}

		void IDisposable.Dispose() { }
	}
}
