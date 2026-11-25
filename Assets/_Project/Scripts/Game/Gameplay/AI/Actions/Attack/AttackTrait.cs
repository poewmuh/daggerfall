using System;
using Game.Gameplay.AI.Traits;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.AI.Actions.Attack
{
	[Serializable]
	public sealed class AttackTrait : AiActionTrait
	{
		[SerializeField, Min(0f)] private float _minInterval = 0.2f;
		[SerializeField, Min(0f)] private float _maxInterval = 0.6f;
		[SerializeField, Min(0f)] private float _firstAttackDelay = 0.25f;

		protected override float DefaultWeight => 1.2f;

		protected override IConsideration[] CreateDefaultConsiderations()
		{
			return new IConsideration[]
			{
				new TargetVisibleConsideration(),
				new DistanceInMetersConsideration(),
			};
		}

		protected override void InstallAction(Entity action, Entity owner, World world)
		{
			world.GetStash<AttackAction>().Set(action, new AttackAction
			{
				MinInterval = _minInterval,
				MaxInterval = Mathf.Max(_minInterval, _maxInterval),
				FirstAttackDelay = _firstAttackDelay,
			});
		}
	}
}
