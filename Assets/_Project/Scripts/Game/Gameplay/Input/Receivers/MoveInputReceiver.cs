using System;
using Game.Application.InputSystem;
using Game.Application.InputSystem.InputActions;
using R3;
using UnityEngine;

namespace Game.Gameplay.Input.Receivers
{
	public class MoveInputReceiver: IMoveInputReceiver, IDisposable
	{
		public Subject<Vector2> MoveState { get; } = new();
		public Subject<Vector2> LookState { get; } = new();

		Observable<Vector2> IMoveInputReceiver.MoveState => MoveState;
		Observable<Vector2> IMoveInputReceiver.LookState => LookState;
		
		private readonly CompositeDisposable _disposables = new();

		public void Init(InputEvents inputEvents)
		{
			inputEvents.OnMove.Subscribe(OnMove).AddTo(_disposables);
			inputEvents.OnLook.Subscribe(OnLook).AddTo(_disposables);
		}

		private void OnMove(InputVectorActionData data)
		{
			MoveState.OnNext(data.Vector);
		}
		
		private void OnLook(InputVectorActionData data)
		{
			LookState.OnNext(data.Vector);
		}
		
		void IDisposable.Dispose() => _disposables.Dispose();
	}
}
