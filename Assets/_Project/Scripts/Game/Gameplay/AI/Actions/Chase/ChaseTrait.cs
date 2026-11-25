using System;
using Game.Gameplay.AI.Traits;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.AI.Actions.Chase
{
	[Serializable]
	public sealed class ChaseTrait : AiActionTrait
	{
		[SerializeField, Min(0.05f)] private float _stoppingDistance = 1.2f;
		[SerializeField, Min(0.05f)] private float _lastKnownStoppingDistance = 0.5f;

		protected override IConsideration[] CreateDefaultConsiderations()
		{
			return new IConsideration[]
			{
				new HasTargetConsideration(),
				new TimeSinceSeenConsideration(),
			};
		}

		protected override void InstallAction(Entity action, Entity owner, World world)
		{
			world.GetStash<ChaseAction>().Set(action, new ChaseAction
			{
				StoppingDistance = _stoppingDistance,
				LastKnownStoppingDistance = _lastKnownStoppingDistance,
			});
		}
	}
}
