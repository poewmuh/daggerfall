using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Combat.Systems
{
	public sealed class AttackSwingSystem : ISystem
	{
		private Filter _filter;

		private Stash<AttackSwing> _swingStash;
		private Stash<AttackTiming> _timingStash;
		private Stash<AttackRecovery> _recoveryStash;
		private Stash<AttackHit> _hitStash;
		private Stash<WeaponDamage> _damageStash;
		private Stash<ChargeAttack> _chargeStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<AttackSwing>()
				.With<AttackTiming>()
				.Build();

			_swingStash = World.GetStash<AttackSwing>();
			_timingStash = World.GetStash<AttackTiming>();
			_recoveryStash = World.GetStash<AttackRecovery>();
			_hitStash = World.GetStash<AttackHit>();
			_damageStash = World.GetStash<WeaponDamage>();
			_chargeStash = World.GetStash<ChargeAttack>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				ref var swing = ref _swingStash.Get(entity);
				ref var timing = ref _timingStash.Get(entity);

				swing.Elapsed += deltaTime;
				var progress = Mathf.Clamp01(swing.Elapsed / timing.SwingDuration);

				if (!swing.HitResolved && progress >= timing.HitTime)
				{
					swing.HitResolved = true;
					_hitStash.Set(entity, new AttackHit { Damage = EvaluateDamage(entity, swing.Charge) });
				}

				if (progress < 1f)
					continue;

				_swingStash.Remove(entity);
				_recoveryStash.Set(entity, new AttackRecovery());
			}
		}

		private float EvaluateDamage(Entity weapon, float charge)
		{
			var baseDamage = _damageStash.Has(weapon) ? _damageStash.Get(weapon).Base : 0f;

			if (!_chargeStash.Has(weapon))
				return baseDamage;

			var maxDamage = Mathf.Max(baseDamage, _chargeStash.Get(weapon).MaxDamage);
			return Mathf.Lerp(baseDamage, maxDamage, charge);
		}

		void IDisposable.Dispose() { }
	}
}
