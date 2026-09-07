using System;
using Game.Application.InputSystem;
using Game.Application.InputSystem.InputActions;
using R3;

namespace Game.Gameplay.Input.Receivers
{
	public sealed class DashInputReceiver : IDashInputReceiver, IDisposable
	{
		public Subject<Unit> DashRequested { get; } = new();
		Observable<Unit> IDashInputReceiver.DashRequested => DashRequested;

		private readonly CompositeDisposable _disposables = new();

		public void Init(InputEvents inputEvents)
		{
			inputEvents.OnDash.Subscribe(_ => DashRequested.OnNext(Unit.Default)).AddTo(_disposables);
		}

		void IDisposable.Dispose() => _disposables.Dispose();
	}
}
