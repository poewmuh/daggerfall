using System;
using Scellecs.Morpeh;

namespace Game.Gameplay.Combat.Systems
{
	public sealed class AttackRecoverySystem : ISystem
	{
		private Filter _filter;
		private Stash<AttackRecovery> _recoveryStash;
		private Stash<AttackTiming> _timingStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<AttackRecovery>()
				.With<AttackTiming>()
				.Build();

			_recoveryStash = World.GetStash<AttackRecovery>();
			_timingStash = World.GetStash<AttackTiming>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				ref var recovery = ref _recoveryStash.Get(entity);
				recovery.Elapsed += deltaTime;

				if (recovery.Elapsed >= _timingStash.Get(entity).RecoveryDuration)
				{
					_recoveryStash.Remove(entity);
				}
			}
		}

		void IDisposable.Dispose() { }
	}
}
