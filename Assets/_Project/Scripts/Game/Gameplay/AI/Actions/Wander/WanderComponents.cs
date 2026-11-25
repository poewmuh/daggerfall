using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.AI.Actions.Wander
{
	public struct WanderAction : IComponent
	{
		public Vector3 Home;
		public float Radius;
		public float WaitMin;
		public float WaitMax;
		public float MaxTravelTime;
		public float StoppingDistance;
		public float Timer;
		public bool Waiting;
	}
}
