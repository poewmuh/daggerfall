using R3;

namespace Game.Gameplay.Input.Receivers
{
	public interface ICombatInputReceiver
	{
		Observable<bool> AttackState { get; }
	}
}
