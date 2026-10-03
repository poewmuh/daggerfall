using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Dash
{
	public enum DashFallbackDirection : byte
	{
		Facing,
		Backward,
	}

	public struct DashParameters : IComponent
	{
		public float Distance;
		public float Duration;
		public float Cooldown;
		public DashFallbackDirection FallbackDirection;
	}

	public struct DashRequest : IComponent { }

	public struct Dashing : IComponent
	{
		public Vector3 Velocity;
		public float TimeRemaining;
	}

	public struct DashCooldown : IComponent
	{
		public float Remaining;
	}
}
