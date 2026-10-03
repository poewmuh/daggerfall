using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Combat
{
	public struct AttackIntent : IComponent
	{
		public bool Held;
	}

	public struct AttackRequest : IComponent { }

	public struct EquippedWeapon : IComponent
	{
		public Entity Value;
	}

	public struct WeaponOwner : IComponent
	{
		public Entity Value;
	}

	public struct AttackTiming : IComponent
	{
		public float SwingDuration;
		public float HitTime;
		public float RecoveryDuration;
	}

	public struct WeaponDamage : IComponent
	{
		public float Base;
	}

	public struct ChargeAttack : IComponent
	{
		public float MaxDuration;
		public float MaxDamage;
	}

	public struct MeleeHit : IComponent
	{
		public float Range;
		public float Radius;
		public LayerMask Mask;
	}

	public struct WeaponPresentationRef : IComponent
	{
		public WeaponPresentation Value;
	}

	public struct AttackCharging : IComponent
	{
		public float Elapsed;
	}

	public struct AttackSwing : IComponent
	{
		public float Elapsed;
		public float Charge;
		public bool HitResolved;
	}

	public struct AttackRecovery : IComponent
	{
		public float Elapsed;
	}

	public struct AttackHit : IComponent
	{
		public float Damage;
	}
}
