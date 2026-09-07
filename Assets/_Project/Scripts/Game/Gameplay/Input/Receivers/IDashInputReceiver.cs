using R3;

namespace Game.Gameplay.Input.Receivers
{
	public interface IDashInputReceiver
	{
		Observable<Unit> DashRequested { get; }
	}
}
