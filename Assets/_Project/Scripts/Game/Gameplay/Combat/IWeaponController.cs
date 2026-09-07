namespace Game.Gameplay.Combat
{
	public interface IWeaponController
	{
		WeaponConfig EquippedWeapon { get; }
		bool TryEquip(WeaponConfig config);
	}
}
