using UnityEngine;

namespace Game.Gameplay.Movement
{
	[RequireComponent(typeof(CharacterController))]
	public sealed class CharacterMotorComponent : MonoBehaviour, ICharacterMotor
	{
		[SerializeField] private Transform _movementReference;
		[SerializeField, Min(0f)] private float _moveSpeed = 4f;
		[SerializeField] private float _gravity = -25f;
		[SerializeField, Min(0f)] private float _groundedVelocity = 2f;

		private CharacterController _characterController;
		private float _verticalVelocity;

		private void Awake()
		{
			_characterController = GetComponent<CharacterController>();
		}

		void ICharacterMotor.Move(Vector2 direction, float deltaTime)
		{
			if (_characterController.isGrounded && _verticalVelocity < 0f)
				_verticalVelocity = -_groundedVelocity;

			_verticalVelocity += _gravity * deltaTime;

			direction = Vector2.ClampMagnitude(direction, 1f);
			var reference = _movementReference == null ? transform : _movementReference;
			var right = Vector3.ProjectOnPlane(reference.right, Vector3.up).normalized;
			var forward = Vector3.Cross(right, Vector3.up).normalized;
			var planarVelocity = right * direction.x + forward * direction.y;
			var velocity = planarVelocity * _moveSpeed + Vector3.up * _verticalVelocity;

			_characterController.Move(velocity * deltaTime);
		}
	}
}
