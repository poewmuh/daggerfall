using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Navigation
{
	public struct NavAgent : IComponent
	{
		public float RepathInterval;
		public float RepathTimer;
		public float CornerReachDistance;
		public float SampleDistance;
	}

	public struct NavDestination : IComponent
	{
		public Vector3 Value;
		public float StoppingDistance;
	}

	public struct NavPath : IComponent
	{
		public Vector3[] Corners;
		public int Count;
		public int Index;
		public Vector3 Destination;
		public bool Valid;
	}

	public struct NavArrived : IComponent { }
}
