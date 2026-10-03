using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Combat.Traits
{
	[Serializable]
	public sealed class WeaponDamageTrait : IWeaponTrait
	{
		[SerializeField, Min(0f)] private float _baseDamage = 10f;

		public void Install(Entity entity, World world)
		{
			world.GetStash<WeaponDamage>().Set(entity, new WeaponDamage { Base = _baseDamage });
		}
	}
}
