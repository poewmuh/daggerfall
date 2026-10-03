using System;
using Scellecs.Morpeh;

namespace Game.Gameplay.Dash.Systems
{
	public sealed class DashCooldownSystem : IFixedSystem
	{
		private Filter _filter;
		private Stash<DashCooldown> _cooldownStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<DashCooldown>()
				.Build();

			_cooldownStash = World.GetStash<DashCooldown>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				ref var cooldown = ref _cooldownStash.Get(entity);
				cooldown.Remaining -= deltaTime;

				if (cooldown.Remaining <= 0f)
				{
					_cooldownStash.Remove(entity);
				}
			}
		}

		void IDisposable.Dispose() { }
	}
}
