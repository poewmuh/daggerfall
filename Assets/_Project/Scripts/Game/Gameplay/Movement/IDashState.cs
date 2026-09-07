namespace Game.Gameplay.Movement
{
	public interface IDashState
	{
		bool IsDashing { get; }
		float CooldownRemaining { get; }
	}
}
