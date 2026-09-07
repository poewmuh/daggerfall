using Game.Gameplay.Core;
using Game.Gameplay.Input.Receivers;
using R3;
using UnityEngine;

namespace Game.Gameplay.Movement
{
	[RequireComponent(typeof(Input.InputReaderComponent))]
	[RequireComponent(typeof(CharacterMotorComponent))]
	public sealed class DashComponent : MonoBehaviour, IBaseComponent, IDashState
	{
		[TriInspector.Required, TriInspector.InlineEditor]
		[SerializeField] private DashConfig _config;

		private ICharacterMotor _motor;
		private Vector2 _moveDirection;
		private float _cooldownRemaining;

		public bool IsDashing => _motor?.IsDashing == true;
		public float CooldownRemaining => _cooldownRemaining;

		void IBaseComponent.Init()
		{
			var inputReader = GetComponent<Input.InputReaderComponent>();
			_motor = GetComponent<ICharacterMotor>();

			((IMoveInputReceiver)inputReader).MoveState
				.Subscribe(direction => _moveDirection = direction)
				.AddTo(this);
			((IDashInputReceiver)inputReader).DashRequested
				.Subscribe(_ => TryDash())
				.AddTo(this);
		}

		private void Update()
		{
			_cooldownRemaining = Mathf.Max(0f, _cooldownRemaining - Time.deltaTime);
		}

		private void TryDash()
		{
			if (_config == null)
			{
				Debug.LogError("[Movement] DashComponent has no DashConfig assigned.", this);
				return;
			}

			if (IsDashing || _cooldownRemaining > 0f)
				return;

			var direction = _moveDirection.sqrMagnitude > 0.0001f
				? _moveDirection
				: Vector2.up;

			_motor.StartDash(direction, _config.Distance, _config.Duration);
			_cooldownRemaining = _config.Cooldown;
		}
	}
}
