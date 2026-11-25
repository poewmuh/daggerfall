using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Damage
{
	public struct DamageEvent : IComponent
	{
		public Entity Target;
		public Entity Source;
		public float Amount;
		public Vector3 Point;
		public Vector3 Direction;
	}

	public struct Health : IComponent
	{
		public float Current;
		public float Max;
	}

	public struct DamageImmunity : IComponent
	{
		public DamageImmunityReason Value;
	}

	[Flags]
	public enum DamageImmunityReason : byte
	{
		None = 0,
		Dash = 1 << 0,
	}

	public struct Dead : IComponent { }

	public struct Died : IComponent { }

	public readonly struct PlayerDiedMessage { }
}
