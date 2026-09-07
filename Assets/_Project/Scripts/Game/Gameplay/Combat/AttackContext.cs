using UnityEngine;

namespace Game.Gameplay.Combat
{
	public readonly struct AttackContext
	{
		public Camera Camera { get; }
		public GameObject Source { get; }
		public float Damage { get; }
		public float Charge { get; }

		public AttackContext(Camera camera, GameObject source, float damage, float charge)
		{
			Camera = camera;
			Source = source;
			Damage = damage;
			Charge = charge;
		}
	}
}
