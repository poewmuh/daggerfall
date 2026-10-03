using System;
using Game.Gameplay.Agent;
using Game.Gameplay.Common;
using Scellecs.Morpeh;
using TriInspector;
using UnityEngine;

namespace Game.Gameplay.Combat.Traits
{
	[Serializable]
	public sealed class WeaponHolderTrait : IAgentTrait
	{
		[Required]
		[SerializeField] private WeaponConfig _startingWeapon;

		public void Install(Entity entity, World world)
		{
			world.GetStash<AttackIntent>().Set(entity);
			world.GetStash<AimRay>().Set(entity);

			var weapon = world.CreateEntity();
			_startingWeapon.Install(weapon, world);

			world.GetStash<WeaponOwner>().Set(weapon, new WeaponOwner { Value = entity });
			world.GetStash<EquippedWeapon>().Set(entity, new EquippedWeapon { Value = weapon });
		}
	}
}
