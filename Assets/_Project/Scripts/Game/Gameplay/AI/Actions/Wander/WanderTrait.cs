using System;
using Game.Gameplay.AI.Traits;
using Game.Gameplay.Common;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.AI.Actions.Wander
{
	[Serializable]
	public sealed class WanderTrait : AiActionTrait
	{
		[SerializeField, Min(0f)] private float _radius = 5f;
		[SerializeField, Min(0f)] private float _waitMin = 1f;
		[SerializeField, Min(0f)] private float _waitMax = 3f;
		[SerializeField, Min(0.5f)] private float _maxTravelTime = 8f;
		[SerializeField, Min(0.05f)] private float _stoppingDistance = 0.3f;

		protected override IConsideration[] CreateDefaultConsiderations()
		{
			return new IConsideration[] { new ConstantConsideration(0.2f) };
		}

		protected override void InstallAction(Entity action, Entity owner, World world)
		{
			var positionStash = world.GetStash<Position>();
			var home = positionStash.Has(owner) ? positionStash.Get(owner).Value : Vector3.zero;

			world.GetStash<WanderAction>().Set(action, new WanderAction
			{
				Home = home,
				Radius = _radius,
				WaitMin = _waitMin,
				WaitMax = Mathf.Max(_waitMin, _waitMax),
				MaxTravelTime = _maxTravelTime,
				StoppingDistance = _stoppingDistance,
				Waiting = true,
			});
		}
	}
}
