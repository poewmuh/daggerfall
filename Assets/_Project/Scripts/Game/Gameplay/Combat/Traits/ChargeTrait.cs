using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Combat.Traits
{
	[Serializable]
	public sealed class ChargeTrait : IWeaponTrait
	{
		[SerializeField, Min(0.01f)] private float _maxChargeDuration = 1.5f;
		[SerializeField, Min(0f)] private float _maxChargedDamage = 30f;

		public void Install(Entity entity, World world)
		{
			world.GetStash<ChargeAttack>().Set(entity, new ChargeAttack
			{
				MaxDuration = _maxChargeDuration,
				MaxDamage = _maxChargedDamage,
			});
		}
	}
}
