using Game.Gameplay.Common.Systems;
using Game.Gameplay.Damage.Systems;
using MessagePipe;
using Scellecs.Morpeh;

namespace Game.Gameplay.Damage
{
	public static class DamageFeature
	{
		public static SystemsGroup Create(World world, IPublisher<PlayerDiedMessage> playerDiedPublisher)
		{
			var group = world.CreateSystemsGroup();

			group.AddSystem(new DamageImmunitySystem());
			group.AddSystem(new DamageApplySystem());
			group.AddSystem(new DeathSystem());
			group.AddSystem(new DestroyOnDeathSystem());
			group.AddSystem(new PlayerDeathNotifySystem(playerDiedPublisher));
			group.AddSystem(new DamageEventCleanupSystem());
			group.AddSystem(new LateComponentCleanupSystem<Died>());

			return group;
		}
	}
}
