using Game.Application.InputSystem;
using Game.Gameplay.Core;
using Game.Gameplay.Input.Receivers;
using R3;
using UnityEngine;
using VContainer;

namespace Game.Gameplay.Input
{
	public class InputReaderComponent : MonoBehaviour, IBaseComponent, IMoveInputReceiver, ICombatInputReceiver
	{
		[Inject] private IInputSystem _inputSystem;
		
		Observable<Vector2> IMoveInputReceiver.MoveState => _moveInputReceiver.MoveState;
		Observable<Vector2> IMoveInputReceiver.LookState => _moveInputReceiver.LookState;
		
		Observable<bool> ICombatInputReceiver.AttackState => _combatInputReceiver.AttackState;
		
		private readonly MoveInputReceiver _moveInputReceiver = new ();
		private readonly CombatInputReceiver _combatInputReceiver = new ();

		void IBaseComponent.Init()
		{
			_moveInputReceiver.Init(_inputSystem.InputEvents);
			_combatInputReceiver.Init(_inputSystem.InputEvents);
			
			Disposable.Combine(_moveInputReceiver, _combatInputReceiver).AddTo(this);
		}
	}
}
