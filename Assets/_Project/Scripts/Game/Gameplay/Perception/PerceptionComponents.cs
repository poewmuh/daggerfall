using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Perception
{
	public struct PerceptionParameters : IComponent
	{
		public float Range;
		public float ProximityRadius;
		public float HalfFovCos;
		public float EyeHeight;
		public float TargetHeight;
		public float MemoryDuration;
		public float Interval;
		public float Timer;
		public LayerMask ObstacleMask;
	}

	public struct PerceivedTarget : IComponent
	{
		public Entity Target;
		public Vector3 LastKnownPosition;
		public float TimeSinceSeen;
		public bool Visible;
		public bool HasTarget;
	}
}
