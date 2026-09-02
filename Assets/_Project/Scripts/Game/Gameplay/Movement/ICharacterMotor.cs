namespace Game.Gameplay.Movement
{
	public interface ICharacterMotor
	{
		void Move(UnityEngine.Vector2 direction, float deltaTime);
	}
}
