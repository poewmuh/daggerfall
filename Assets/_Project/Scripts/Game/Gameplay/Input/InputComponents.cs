using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Input
{
	public struct PlayerControlled : IComponent { }

	public struct InputMoveDirection : IComponent
	{
		public Vector2 Value;
	}
}
