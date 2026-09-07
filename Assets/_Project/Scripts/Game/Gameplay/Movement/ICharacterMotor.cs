namespace Game.Gameplay.Movement
{
	public interface ICharacterMotor
	{
		bool IsDashing { get; }
		void Move(UnityEngine.Vector2 direction, float deltaTime);
		void StartDash(UnityEngine.Vector2 direction, float distance, float duration);
	}
}
