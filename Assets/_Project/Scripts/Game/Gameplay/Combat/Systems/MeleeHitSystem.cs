using System;
using Game.Gameplay.Common;
using Game.Gameplay.Damage;
using Game.Gameplay.View;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Combat.Systems
{
	public sealed class MeleeHitSystem : ISystem
	{
		private Filter _filter;

		private Stash<AttackHit> _hitStash;
		private Stash<MeleeHit> _meleeStash;
		private Stash<WeaponOwner> _ownerStash;
		private Stash<AimRay> _aimStash;
		private Stash<DamageEvent> _damageEventStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<AttackHit>()
				.With<MeleeHit>()
				.With<WeaponOwner>()
				.Build();

			_hitStash = World.GetStash<AttackHit>();
			_meleeStash = World.GetStash<MeleeHit>();
			_ownerStash = World.GetStash<WeaponOwner>();
			_aimStash = World.GetStash<AimRay>();
			_damageEventStash = World.GetStash<DamageEvent>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var weapon in _filter)
			{
				var owner = _ownerStash.Get(weapon).Value;

				if (World.IsDisposed(owner) || !_aimStash.Has(owner))
					continue;

				ref var melee = ref _meleeStash.Get(weapon);
				ref var aim = ref _aimStash.Get(owner);

				if (!Physics.SphereCast(aim.Origin, melee.Radius, aim.Direction, out var hit, melee.Range, melee.Mask, QueryTriggerInteraction.Ignore))
					continue;

				var target = hit.collider.GetComponentInParent<EntityInitializer>();

				if (target == null || target.World != World || target.Entity == owner || World.IsDisposed(target.Entity))
					continue;

				_damageEventStash.Set(World.CreateEntity(), new DamageEvent
				{
					Target = target.Entity,
					Source = owner,
					Amount = _hitStash.Get(weapon).Damage,
					Point = hit.point,
					Direction = aim.Direction,
				});
			}
		}

		void IDisposable.Dispose() { }
	}
}
