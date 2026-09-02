using Game.Gameplay.Core;
using Game.Gameplay.Input.Receivers;
using R3;
using UnityEngine;

namespace Game.Gameplay.Movement
{
	[RequireComponent(typeof(Input.InputReaderComponent))]
	[RequireComponent(typeof(CharacterMotorComponent))]
	public sealed class MovementComponent : MonoBehaviour, IBaseComponent
	{
		private Vector2 _moveDirection;
		private ICharacterMotor _motor;
		private bool _initialized;

		void IBaseComponent.Init()
		{
			var inputReader = GetComponent<IMoveInputReceiver>();
			_motor = GetComponent<ICharacterMotor>();

			inputReader.MoveState.Subscribe(value => _moveDirection = value).AddTo(this);
			_initialized = true;
		}

		private void Update()
		{
			if (!_initialized)
				return;

			_motor.Move(_moveDirection, Time.deltaTime);
		}
	}
}
