using System;
using Game.Gameplay.Movement;
using Scellecs.Morpeh;

namespace Game.Gameplay.Dash.Systems
{
	public sealed class DashEndSystem : IFixedSystem
	{
		private Filter _filter;

		private Stash<Dashing> _dashingStash;
		private Stash<DashParameters> _parametersStash;
		private Stash<DashCooldown> _cooldownStash;
		private Stash<MovementBlockers> _blockersStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<Dashing>()
				.With<DashParameters>()
				.With<MovementBlockers>()
				.Build();

			_dashingStash = World.GetStash<Dashing>();
			_parametersStash = World.GetStash<DashParameters>();
			_cooldownStash = World.GetStash<DashCooldown>();
			_blockersStash = World.GetStash<MovementBlockers>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				ref var dashing = ref _dashingStash.Get(entity);
				dashing.TimeRemaining -= deltaTime;

				if (dashing.TimeRemaining > 0f)
					continue;

				_dashingStash.Remove(entity);
				_blockersStash.Get(entity).Value &= ~MovementBlockReason.Dash;

				var cooldown = _parametersStash.Get(entity).Cooldown;

				if (cooldown > 0f)
				{
					_cooldownStash.Set(entity, new DashCooldown { Remaining = cooldown });
				}
			}
		}

		void IDisposable.Dispose() { }
	}
}
