using System;
using TriInspector;
using UnityEngine;

namespace Game.Gameplay.Combat
{
	[Serializable]
	public sealed class MeleeAttackDelivery : IAttackDelivery
	{
		[Title("Hit Volume")]
		[SerializeField, Min(0f)] private float _range = 2f;
		[SerializeField, Min(0f)] private float _radius = 0.3f;
		[SerializeField] private LayerMask _hitMask = ~(1 << 6);

		bool IAttackDelivery.Execute(in AttackContext context)
		{
			var cameraTransform = context.Camera.transform;
			if (!Physics.SphereCast(
				    cameraTransform.position,
				    _radius,
				    cameraTransform.forward,
				    out var hit,
				    _range,
				    _hitMask,
				    QueryTriggerInteraction.Ignore))
				return false;

			var damageable = hit.collider.GetComponentInParent<IDamageable>();
			if (damageable == null)
				return false;

			var damageInfo = new DamageInfo(context.Damage, hit.point, cameraTransform.forward, context.Source);
			damageable.ApplyDamage(in damageInfo);
			return true;
		}
	}
}
