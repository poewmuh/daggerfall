using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Combat.Traits
{
	[Serializable]
	public sealed class MeleeHitTrait : IWeaponTrait
	{
		[SerializeField, Min(0f)] private float _range = 2f;
		[SerializeField, Min(0f)] private float _radius = 0.3f;
		[SerializeField] private LayerMask _hitMask = ~0;

		public void Install(Entity entity, World world)
		{
			world.GetStash<MeleeHit>().Set(entity, new MeleeHit
			{
				Range = _range,
				Radius = _radius,
				Mask = _hitMask,
			});
		}
	}
}
