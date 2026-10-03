using Game.Gameplay.Entities;
using TriInspector;
using UnityEngine;

namespace Game.Gameplay.Combat
{
	[CreateAssetMenu(fileName = "WeaponConfig", menuName = "_Project/Gameplay/Weapon Config")]
	[DeclareBoxGroup("Identity", Title = "Identity")]
	public sealed class WeaponConfig : EntityConfig<IWeaponTrait>
	{
		[Group("Identity")]
		[SerializeField] private string _id = "weapon";
		[Group("Identity")]
		[SerializeField] private string _displayName = "Weapon";
		[Group("Identity")]
		[SerializeField] private WeaponType _weaponType;

		public string Id => _id;
		public string DisplayName => _displayName;
		public WeaponType WeaponType => _weaponType;
	}
}
