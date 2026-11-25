using System;
using Game.Gameplay.Navigation;
using Scellecs.Morpeh;

namespace Game.Gameplay.AI.Systems
{
	public sealed class AiSelectionSystem : ISystem
	{
		private Filter _brains;
		private Filter _actions;

		private Stash<AiBrain> _brainStash;
		private Stash<AiAction> _actionStash;
		private Stash<AiActive> _activeStash;
		private Stash<AiActivated> _activatedStash;
		private Stash<NavDestination> _destinationStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_brains = World.Filter
				.With<AiBrain>()
				.Build();

			_actions = World.Filter
				.With<AiAction>()
				.Build();

			_brainStash = World.GetStash<AiBrain>();
			_actionStash = World.GetStash<AiAction>();
			_activeStash = World.GetStash<AiActive>();
			_activatedStash = World.GetStash<AiActivated>();
			_destinationStash = World.GetStash<NavDestination>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var owner in _brains)
			{
				ref var brain = ref _brainStash.Get(owner);
				brain.BestScore = 0f;
				brain.BestAction = default;
			}

			foreach (var action in _actions)
			{
				ref var data = ref _actionStash.Get(action);

				if (World.IsDisposed(data.Owner) || !_brainStash.Has(data.Owner))
					continue;

				ref var brain = ref _brainStash.Get(data.Owner);
				var isActive = brain.HasActiveAction && brain.ActiveAction == action;
				var score = data.Score > 0f && isActive ? data.Score + brain.Inertia : data.Score;

				if (score > brain.BestScore)
				{
					brain.BestScore = score;
					brain.BestAction = action;
				}
			}

			foreach (var owner in _brains)
			{
				ref var brain = ref _brainStash.Get(owner);
				var hasBest = brain.BestScore > 0f;

				if (hasBest == brain.HasActiveAction && (!hasBest || brain.BestAction == brain.ActiveAction))
					continue;

				Switch(owner, ref brain, hasBest);
			}
		}

		private void Switch(Entity owner, ref AiBrain brain, bool hasBest)
		{
			if (brain.HasActiveAction && !World.IsDisposed(brain.ActiveAction))
				_activeStash.Remove(brain.ActiveAction);

			_destinationStash.Remove(owner);

			brain.HasActiveAction = hasBest;
			brain.ActiveAction = hasBest ? brain.BestAction : default;

			if (!hasBest)
				return;

			_activeStash.Set(brain.ActiveAction);
			_activatedStash.Set(brain.ActiveAction);
		}

		void IDisposable.Dispose() { }
	}
}
