using Scellecs.Morpeh;

namespace Game.Gameplay.AI
{
	public struct AiBrain : IComponent
	{
		public Entity ActiveAction;
		public bool HasActiveAction;
		public float Inertia;
		public Entity BestAction;
		public float BestScore;
	}

	public struct AiContext : IComponent
	{
		public bool HasTarget;
		public bool TargetVisible;
		public float DistanceToTarget;
		public float PerceptionRange;
		public float TimeSinceSeen;
		public float MemoryDuration;
		public float HealthRatio;
	}

	public struct AiAction : IComponent
	{
		public Entity Owner;
		public float Weight;
		public IConsideration[] Considerations;
		public float Score;
	}

	public struct AiActive : IComponent { }

	public struct AiActivated : IComponent { }
}
