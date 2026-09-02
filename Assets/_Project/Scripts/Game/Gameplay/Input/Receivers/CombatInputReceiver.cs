using System;
using Game.Application.InputSystem;
using Game.Application.InputSystem.InputActions;
using R3;
using UnityEngine.InputSystem;

namespace Game.Gameplay.Input.Receivers
{
	public class CombatInputReceiver : ICombatInputReceiver, IDisposable
	{
		public Subject<bool> AttackState { get; } = new();
		Observable<bool> ICombatInputReceiver.AttackState => AttackState;
		
		private readonly CompositeDisposable _disposables = new();

		public void Init(InputEvents inputEvents)
		{
			inputEvents.OnAttack.Subscribe(OnAttack).AddTo(_disposables);
		}

		private void OnAttack(InputActionData data)
		{
			if (data.Phase == InputActionPhase.Started) AttackState.OnNext(true);
			if (data.Phase == InputActionPhase.Canceled) AttackState.OnNext(false);
		}
		
		void IDisposable.Dispose() => _disposables.Dispose();
	}
}
