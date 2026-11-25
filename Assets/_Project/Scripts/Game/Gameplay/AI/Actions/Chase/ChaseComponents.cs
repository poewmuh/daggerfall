using Scellecs.Morpeh;

namespace Game.Gameplay.AI.Actions.Chase
{
	public struct ChaseAction : IComponent
	{
		public float StoppingDistance;
		public float LastKnownStoppingDistance;
	}
}
