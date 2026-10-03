using System;
using Game.Application.InputSystem;
using Game.Application.InputSystem.InputActions;
using Game.Gameplay.Dash;
using R3;
using Scellecs.Morpeh;

namespace Game.Gameplay.Input.Systems
{
	public class DashInputApplyInitializer : IInitializer
	{
		private readonly IInputSystem _inputSystem;
		
		private Filter _filter;
		
		private Stash<DashRequest> _dashRequestStash;
		
		private readonly CompositeDisposable _disposables = new ();
		
		public World World { get; set; }

		public DashInputApplyInitializer(IInputSystem inputSystem)
		{
			_inputSystem = inputSystem;
		}

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<PlayerControlled>()
				.Build();

			_dashRequestStash = World.GetStash<DashRequest>();
			
			_inputSystem.InputEvents.OnDash.Subscribe(OnDash).AddTo(_disposables);
		}

		private void OnDash(InputActionData data)
		{
			foreach (var entity in _filter)
			{
				_dashRequestStash.Set(entity);
			}
		}

		void IDisposable.Dispose()
		{
			_disposables.Dispose();
		}
	}
}
