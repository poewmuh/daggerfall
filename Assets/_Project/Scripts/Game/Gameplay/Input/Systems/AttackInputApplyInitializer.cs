using System;
using Game.Application.InputSystem;
using Game.Application.InputSystem.InputActions;
using Game.Gameplay.Combat;
using R3;
using Scellecs.Morpeh;
using UnityEngine.InputSystem;

namespace Game.Gameplay.Input.Systems
{
	public sealed class AttackInputApplyInitializer : IInitializer
	{
		private readonly IInputSystem _inputSystem;
		private readonly CompositeDisposable _disposables = new();

		private Filter _filter;

		private Stash<AttackIntent> _intentStash;
		private Stash<AttackRequest> _requestStash;

		public World World { get; set; }

		public AttackInputApplyInitializer(IInputSystem inputSystem)
		{
			_inputSystem = inputSystem;
		}

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<PlayerControlled>()
				.With<AttackIntent>()
				.Build();

			_intentStash = World.GetStash<AttackIntent>();
			_requestStash = World.GetStash<AttackRequest>();

			_inputSystem.InputEvents.OnAttack.Subscribe(OnAttack).AddTo(_disposables);
		}

		private void OnAttack(InputActionData data)
		{
			switch (data.Phase)
			{
				case InputActionPhase.Started:
					Apply(true);
					break;
				case InputActionPhase.Canceled:
					Apply(false);
					break;
			}
		}

		private void Apply(bool held)
		{
			foreach (var entity in _filter)
			{
				_intentStash.Get(entity).Held = held;

				if (held)
					_requestStash.Set(entity);
			}
		}

		void IDisposable.Dispose()
		{
			_disposables.Dispose();
		}
	}
}
