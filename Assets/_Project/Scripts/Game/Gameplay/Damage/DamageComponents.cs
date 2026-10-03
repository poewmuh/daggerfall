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
}
