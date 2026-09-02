using R3;
using UnityEngine;

namespace Game.Gameplay.Input.Receivers
{
	public interface IMoveInputReceiver
	{
		Observable<Vector2> MoveState { get; }
		Observable<Vector2> LookState { get; }
	}
}
