using Game.Gameplay.Combat.Systems;
using Game.Gameplay.Common.Systems;
using MessagePipe;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Combat
{
	public static class CombatFeature
	{
		public static SystemsGroup Create(World world, Camera camera, IPublisher<WeaponPresentationMessage> presentationPublisher)
		{
			var group = world.CreateSystemsGroup();

			group.AddSystem(new CameraAimSystem(camera));
			group.AddSystem(new AttackRecoverySystem());
			group.AddSystem(new AttackSwingSystem());
			group.AddSystem(new MeleeHitSystem());
			group.AddSystem(new AttackChargeSystem());
			group.AddSystem(new AttackStartSystem());
			group.AddSystem(new WeaponPresentationSystem(presentationPublisher));
			group.AddSystem(new ComponentCleanupSystem<AttackHit>());
			group.AddSystem(new ComponentCleanupSystem<AttackRequest>());

			return group;
		}
	}
}
