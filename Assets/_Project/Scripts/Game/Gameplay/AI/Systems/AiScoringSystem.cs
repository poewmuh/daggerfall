using System;
using Game.Gameplay.Damage;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.AI.Systems
{
	public sealed class AiScoringSystem : ISystem
	{
		private Filter _filter;

		private Stash<AiAction> _actionStash;
		private Stash<AiContext> _contextStash;
		private Stash<Dead> _deadStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<AiAction>()
				.Build();

			_actionStash = World.GetStash<AiAction>();
			_contextStash = World.GetStash<AiContext>();
			_deadStash = World.GetStash<Dead>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				ref var action = ref _actionStash.Get(entity);
				var owner = action.Owner;

				if (World.IsDisposed(owner) || _deadStash.Has(owner) || !_contextStash.Has(owner))
				{
					action.Score = 0f;
					continue;
				}

				action.Score = Evaluate(in action, in _contextStash.Get(owner));
			}
		}

		private static float Evaluate(in AiAction action, in AiContext context)
		{
			var score = action.Weight;

			foreach (var consideration in action.Considerations)
			{
				score *= Mathf.Clamp01(consideration.Evaluate(in context));

				if (score <= 0f)
					return 0f;
			}

			return score;
		}

		void IDisposable.Dispose() { }
	}
}
