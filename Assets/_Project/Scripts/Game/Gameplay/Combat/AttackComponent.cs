using Game.Gameplay.Core;
using Game.Gameplay.Input.Receivers;
using MessagePipe;
using R3;
using UnityEngine;
using VContainer;

namespace Game.Gameplay.Combat
{
	[RequireComponent(typeof(Input.InputReaderComponent))]
	public sealed class AttackComponent : MonoBehaviour, IBaseComponent, IWeaponController
	{
		[TriInspector.Required, TriInspector.InlineEditor]
		[SerializeField] private WeaponConfig _weaponConfig;

		[Inject] private IPublisher<WeaponPresentationMessage> _presentationPublisher;
		[Inject] private Camera _camera;

		private WeaponActionPhase _phase;
		private float _phaseElapsed;
		private float _charge;
		private bool _hitResolved;

		WeaponConfig IWeaponController.EquippedWeapon => _weaponConfig;

		void IBaseComponent.Init()
		{
			var inputReader = GetComponent<ICombatInputReceiver>();
			inputReader.AttackState.DistinctUntilChanged().Subscribe(OnAttackStateChanged).AddTo(this);
			PublishState(WeaponActionPhase.Idle, 0f);
		}

		bool IWeaponController.TryEquip(WeaponConfig config)
		{
			if (config == null || _phase != WeaponActionPhase.Idle)
				return false;

			_weaponConfig = config;
			_charge = 0f;
			PublishState(WeaponActionPhase.Idle, 0f);
			return true;
		}

		private void Update()
		{
			switch (_phase)
			{
				case WeaponActionPhase.Charging:
					UpdateCharging();
					break;
				case WeaponActionPhase.Swing:
					UpdateSwing();
					break;
				case WeaponActionPhase.Recovery:
					UpdateRecovery();
					break;
			}
		}

		private void OnAttackStateChanged(bool isPressed)
		{
			if (isPressed && _phase == WeaponActionPhase.Idle)
			{
				if (_weaponConfig.InputMode == AttackInputMode.Charge)
					ChangePhase(WeaponActionPhase.Charging);
				else
					BeginSwing();
			}
			else if (!isPressed && _phase == WeaponActionPhase.Charging)
			{
				BeginSwing();
			}
		}

		private void BeginSwing()
		{
			_phase = WeaponActionPhase.Swing;
			_phaseElapsed = 0f;
			_hitResolved = false;
			PublishState(_phase, 0f);
		}

		private void UpdateCharging()
		{
			_phaseElapsed += Time.deltaTime;
			_charge = Mathf.Clamp01(_phaseElapsed / _weaponConfig.MaxChargeDuration);
			PublishState(_phase, _charge);
		}

		private void UpdateSwing()
		{
			_phaseElapsed += Time.deltaTime;
			var progress = Mathf.Clamp01(_phaseElapsed / _weaponConfig.SwingDuration);

			if (!_hitResolved && progress >= _weaponConfig.HitTime)
			{
				_hitResolved = true;
				var context = new AttackContext(_camera, gameObject, _weaponConfig.EvaluateDamage(_charge), _charge);
				if (_weaponConfig.Delivery == null)
					Debug.LogError($"[Combat] Weapon '{_weaponConfig.name}' has no attack delivery configured.", _weaponConfig);
				else
					_weaponConfig.Delivery.Execute(in context);
			}

			PublishState(_phase, progress);
			if (progress >= 1f)
				ChangePhase(WeaponActionPhase.Recovery);
		}

		private void UpdateRecovery()
		{
			_phaseElapsed += Time.deltaTime;
			var recoveryDuration = _weaponConfig.RecoveryDuration;
			var progress = recoveryDuration <= 0f ? 1f : Mathf.Clamp01(_phaseElapsed / recoveryDuration);
			PublishState(_phase, progress);

			if (progress >= 1f)
			{
				_charge = 0f;
				ChangePhase(WeaponActionPhase.Idle);
			}
		}

		private void ChangePhase(WeaponActionPhase phase)
		{
			_phase = phase;
			_phaseElapsed = 0f;
			PublishState(phase, 0f);
		}

		private void PublishState(WeaponActionPhase phase, float progress)
		{
			_presentationPublisher.Publish(new WeaponPresentationMessage(_weaponConfig, phase, progress, _charge));
		}
	}
}
