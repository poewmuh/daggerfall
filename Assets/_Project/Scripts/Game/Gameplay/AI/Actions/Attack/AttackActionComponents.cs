using Scellecs.Morpeh;

namespace Game.Gameplay.AI.Actions.Attack
{
	public struct AttackAction : IComponent
	{
		public float MinInterval;
		public float MaxInterval;
		public float FirstAttackDelay;
		public float Timer;
	}
}
