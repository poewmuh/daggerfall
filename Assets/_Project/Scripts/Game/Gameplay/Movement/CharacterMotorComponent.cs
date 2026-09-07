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
		private Vector3 _dashDirection;
		private float _dashSpeed;
		private float _dashTimeRemaining;

		bool ICharacterMotor.IsDashing => _dashTimeRemaining > 0f;

		private void Awake()
		{
			_characterController = GetComponent<CharacterController>();
		}

		void ICharacterMotor.Move(Vector2 direction, float deltaTime)
		{
			if (_characterController.isGrounded && _verticalVelocity < 0f)
				_verticalVelocity = -_groundedVelocity;

			_verticalVelocity += _gravity * deltaTime;

			if (_dashTimeRemaining > 0f)
			{
				var dashDeltaTime = Mathf.Min(deltaTime, _dashTimeRemaining);
				var dashDisplacement = _dashDirection * (_dashSpeed * dashDeltaTime);
				var verticalDisplacement = Vector3.up * (_verticalVelocity * deltaTime);
				_characterController.Move(dashDisplacement + verticalDisplacement);
				_dashTimeRemaining -= dashDeltaTime;
				return;
			}

			var planarVelocity = GetPlanarDirection(direction) * _moveSpeed;
			var velocity = planarVelocity + Vector3.up * _verticalVelocity;
			_characterController.Move(velocity * deltaTime);
		}

		void ICharacterMotor.StartDash(Vector2 direction, float distance, float duration)
		{
			if (distance <= 0f || duration <= 0f)
				return;

			_dashDirection = GetPlanarDirection(direction).normalized;
			if (_dashDirection.sqrMagnitude <= Mathf.Epsilon)
				return;

			_dashSpeed = distance / duration;
			_dashTimeRemaining = duration;
		}

		private Vector3 GetPlanarDirection(Vector2 direction)
		{
			direction = Vector2.ClampMagnitude(direction, 1f);
			var reference = _movementReference == null ? transform : _movementReference;
			var right = Vector3.ProjectOnPlane(reference.right, Vector3.up).normalized;
			var forward = Vector3.Cross(right, Vector3.up).normalized;
			return right * direction.x + forward * direction.y;
		}
	}
}
