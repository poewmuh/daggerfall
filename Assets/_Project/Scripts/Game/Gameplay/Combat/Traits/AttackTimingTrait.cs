using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Combat.Traits
{
	[Serializable]
	public sealed class AttackTimingTrait : IWeaponTrait
	{
		[SerializeField, Min(0.01f)] private float _swingDuration = 0.3f;
		[SerializeField, Range(0f, 1f)] private float _hitTime = 0.45f;
		[SerializeField, Min(0f)] private float _recoveryDuration = 0.35f;

		public void Install(Entity entity, World world)
		{
			world.GetStash<AttackTiming>().Set(entity, new AttackTiming
			{
				SwingDuration = _swingDuration,
				HitTime = _hitTime,
				RecoveryDuration = _recoveryDuration,
			});
		}
	}
}
