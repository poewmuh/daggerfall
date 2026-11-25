namespace Game.Gameplay.AI
{
	public interface IConsideration
	{
		float Evaluate(in AiContext context);
	}
}
