using System;
using Scellecs.Morpeh;
using TriInspector;
using UnityEngine;

namespace Game.Gameplay.Combat.Traits
{
	[Serializable]
	public sealed class WeaponPresentationTrait : IWeaponTrait
	{
		[Required, InlineEditor]
		[SerializeField] private WeaponPresentation _presentation;

		public void Install(Entity entity, World world)
		{
			world.GetStash<WeaponPresentationRef>().Set(entity, new WeaponPresentationRef { Value = _presentation });
		}
	}
}
