using System;
using Game.Gameplay.Actions;
using Scellecs.Morpeh;

namespace Game.Gameplay.Combat.Systems
{
	public sealed class AttackInterruptSystem : ISystem
	{
		private Filter _filter;

		private Stash<WeaponOwner> _ownerStash;
		private Stash<ActionBlockers> _actionBlockersStash;
		private Stash<AttackCharging> _chargingStash;
		private Stash<AttackSwing> _swingStash;
		private Stash<AttackRecovery> _recoveryStash;
		private Stash<AttackHit> _hitStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<WeaponOwner>()
				.With<AttackTiming>()
				.Build();

			_ownerStash = World.GetStash<WeaponOwner>();
			_actionBlockersStash = World.GetStash<ActionBlockers>();
			_chargingStash = World.GetStash<AttackCharging>();
			_swingStash = World.GetStash<AttackSwing>();
			_recoveryStash = World.GetStash<AttackRecovery>();
			_hitStash = World.GetStash<AttackHit>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var weapon in _filter)
			{
				if (!IsOwnerBlocked(_ownerStash.Get(weapon).Value))
					continue;

				_chargingStash.Remove(weapon);
				_swingStash.Remove(weapon);
				_recoveryStash.Remove(weapon);
				_hitStash.Remove(weapon);
			}
		}

		private bool IsOwnerBlocked(Entity owner)
		{
			return !World.IsDisposed(owner)
				&& _actionBlockersStash.Has(owner)
				&& _actionBlockersStash.Get(owner).Value != ActionBlockReason.None;
		}

		void IDisposable.Dispose() { }
	}
}
