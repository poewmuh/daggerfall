using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Common
{
	public struct Position : IComponent
	{
		public Vector3 Value;
	}

	public struct AimRay : IComponent
	{
		public Vector3 Origin;
		public Vector3 Direction;
	}
}
