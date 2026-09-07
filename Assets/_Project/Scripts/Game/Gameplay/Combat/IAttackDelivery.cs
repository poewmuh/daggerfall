namespace Game.Gameplay.Combat
{
	public interface IAttackDelivery
	{
		bool Execute(in AttackContext context);
	}
}
