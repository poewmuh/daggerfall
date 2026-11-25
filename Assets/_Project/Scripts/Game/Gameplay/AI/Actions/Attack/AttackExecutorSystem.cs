using System;
using Game.Gameplay.Actions;
using Game.Gameplay.Combat;
using Game.Gameplay.Common;
using Game.Gameplay.Movement;
using Game.Gameplay.Navigation;
using Game.Gameplay.Perception;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.AI.Actions.Attack
{
	public sealed class AttackExecutorSystem : ISystem
	{
		private const float MinSqrMagnitude = 0.0001f;

		private Filter _filter;

		private Stash<AiAction> _actionStash;
		private Stash<AiActivated> _activatedStash;
		private Stash<AttackAction> _attackStash;
		private Stash<PerceivedTarget> _targetStash;
		private Stash<Position> _positionStash;
		private Stash<FacingDirection> _facingStash;
		private Stash<NavDestination> _destinationStash;
		private Stash<EquippedWeapon> _equippedStash;
		private Stash<ActionBlockers> _actionBlockersStash;
		private Stash<AttackRequest> _requestStash;
		private Stash<AttackCharging> _chargingStash;
		private Stash<AttackSwing> _swingStash;
		private Stash<AttackRecovery> _recoveryStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<AiAction>()
				.With<AiActive>()
				.With<AttackAction>()
				.Build();

			_actionStash = World.GetStash<AiAction>();
			_activatedStash = World.GetStash<AiActivated>();
			_attackStash = World.GetStash<AttackAction>();
			_targetStash = World.GetStash<PerceivedTarget>();
			_positionStash = World.GetStash<Position>();
			_facingStash = World.GetStash<FacingDirection>();
			_destinationStash = World.GetStash<NavDestination>();
			_equippedStash = World.GetStash<EquippedWeapon>();
			_actionBlockersStash = World.GetStash<ActionBlockers>();
			_requestStash = World.GetStash<AttackRequest>();
			_chargingStash = World.GetStash<AttackCharging>();
			_swingStash = World.GetStash<AttackSwing>();
			_recoveryStash = World.GetStash<AttackRecovery>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var action in _filter)
			{
				var owner = _actionStash.Get(action).Owner;

				if (World.IsDisposed(owner) || !_equippedStash.Has(owner))
					continue;

				ref var attack = ref _attackStash.Get(action);

				if (_activatedStash.Has(action))
					attack.Timer = attack.FirstAttackDelay;

				_destinationStash.Remove(owner);
				FaceTarget(owner);

				var weapon = _equippedStash.Get(owner).Value;

				if (!IsWeaponIdle(weapon) || IsBlocked(owner))
					continue;

				attack.Timer -= deltaTime;

				if (attack.Timer > 0f)
					continue;

				_requestStash.Set(owner);
				attack.Timer = UnityEngine.Random.Range(attack.MinInterval, attack.MaxInterval);
			}
		}

		private void FaceTarget(Entity owner)
		{
			if (!_targetStash.Has(owner) || !_facingStash.Has(owner) || !_positionStash.Has(owner))
				return;

			ref var target = ref _targetStash.Get(owner);

			if (!target.HasTarget)
				return;

			var targetPosition = !World.IsDisposed(target.Target) && _positionStash.Has(target.Target)
				? _positionStash.Get(target.Target).Value
				: target.LastKnownPosition;

			var direction = PlanarMath.Flatten(targetPosition - _positionStash.Get(owner).Value);

			if (direction.sqrMagnitude > MinSqrMagnitude)
				_facingStash.Get(owner).Value = direction.normalized;
		}

		private bool IsWeaponIdle(Entity weapon)
		{
			return !World.IsDisposed(weapon)
				&& !_chargingStash.Has(weapon)
				&& !_swingStash.Has(weapon)
				&& !_recoveryStash.Has(weapon);
		}

		private bool IsBlocked(Entity owner)
		{
			return _actionBlockersStash.Has(owner) && _actionBlockersStash.Get(owner).Value != ActionBlockReason.None;
		}

		void IDisposable.Dispose() { }
	}
}
