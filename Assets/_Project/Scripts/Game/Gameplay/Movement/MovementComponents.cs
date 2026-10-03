using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Movement
{
	public struct MoveSpeed : IComponent
	{
		public float Value;
	}
	
	public struct Velocity : IComponent
	{
		public Vector3 Value;
	}

	public struct MoveDirection : IComponent
	{
		public Vector3 Value;
	}

	public struct FacingDirection : IComponent
	{
		public Vector3 Value;
	}

	public struct RigidbodyComponent : IComponent
	{
		public Rigidbody Value;
	}

	public struct MovementBlockers : IComponent
	{
		public MovementBlockReason Value;
	}
	
	public struct CanMove : IComponent { }

	[Flags]
	public enum MovementBlockReason : byte
	{
		None = 0,
		Dash = 1 << 0,
		Dead = 1 << 1,
	}
}
