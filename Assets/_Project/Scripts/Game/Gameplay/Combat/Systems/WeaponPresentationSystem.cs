using System;
using Game.Gameplay.Input;
using MessagePipe;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Combat.Systems
{
	public sealed class WeaponPresentationSystem : ISystem
	{
		private readonly IPublisher<WeaponPresentationMessage> _publisher;

		private Filter _filter;

		private Stash<EquippedWeapon> _equippedStash;
		private Stash<WeaponPresentationRef> _presentationStash;
		private Stash<AttackTiming> _timingStash;
		private Stash<ChargeAttack> _chargeStash;
		private Stash<AttackCharging> _chargingStash;
		private Stash<AttackSwing> _swingStash;
		private Stash<AttackRecovery> _recoveryStash;

		public World World { get; set; }

		public WeaponPresentationSystem(IPublisher<WeaponPresentationMessage> publisher)
		{
			_publisher = publisher;
		}

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<PlayerControlled>()
				.With<EquippedWeapon>()
				.Build();

			_equippedStash = World.GetStash<EquippedWeapon>();
			_presentationStash = World.GetStash<WeaponPresentationRef>();
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

				if (World.IsDisposed(weapon) || !_presentationStash.Has(weapon) || !_timingStash.Has(weapon))
					continue;

				_publisher.Publish(BuildMessage(weapon));
			}
		}

		private WeaponPresentationMessage BuildMessage(Entity weapon)
		{
			var presentation = _presentationStash.Get(weapon).Value;
			ref var timing = ref _timingStash.Get(weapon);

			if (_chargingStash.Has(weapon))
			{
				var charge = Mathf.Clamp01(_chargingStash.Get(weapon).Elapsed / _chargeStash.Get(weapon).MaxDuration);
				return new WeaponPresentationMessage(presentation, WeaponActionPhase.Charging, charge, charge);
			}

			if (_swingStash.Has(weapon))
			{
				ref var swing = ref _swingStash.Get(weapon);
				var progress = Mathf.Clamp01(swing.Elapsed / timing.SwingDuration);
				return new WeaponPresentationMessage(presentation, WeaponActionPhase.Swing, progress, swing.Charge);
			}

			if (_recoveryStash.Has(weapon))
			{
				var progress = timing.RecoveryDuration <= 0f
					? 1f
					: Mathf.Clamp01(_recoveryStash.Get(weapon).Elapsed / timing.RecoveryDuration);
				return new WeaponPresentationMessage(presentation, WeaponActionPhase.Recovery, progress, 0f);
			}

			return new WeaponPresentationMessage(presentation, WeaponActionPhase.Idle, 0f, 0f);
		}

		void IDisposable.Dispose() { }
	}
}
