using System;
using Game.Application.InputSystem;
using Game.Application.InputSystem.InputActions;
using R3;
using Scellecs.Morpeh;

namespace Game.Gameplay.Input.Systems
{
	public class MoveInputApplyInitializer : IInitializer
	{
		private readonly IInputSystem _inputSystem;
		
		private Filter _inputMoveFilter;
		
		private Stash<InputMoveDirection> _inputMoveStash;
		
		private readonly CompositeDisposable _disposables = new ();
		
		public World World { get; set; }

		public MoveInputApplyInitializer(IInputSystem inputSystem)
		{
			_inputSystem = inputSystem;
		}

		void IInitializer.OnAwake()
		{
			_inputMoveFilter = World.Filter
				.With<InputMoveDirection>()
				.Build();

			_inputMoveStash = World.GetStash<InputMoveDirection>();
			
			_inputSystem.InputEvents.OnMove.Subscribe(OnMove).AddTo(_disposables);
		}

		private void OnMove(InputVectorActionData data)
		{
			foreach (var moveInputEntity in _inputMoveFilter)
			{
				ref var moveInput = ref _inputMoveStash.Get(moveInputEntity);
				moveInput.Value = data.Vector;
			}
		}

		void IDisposable.Dispose()
		{
			_disposables.Dispose();
		}
	}
}