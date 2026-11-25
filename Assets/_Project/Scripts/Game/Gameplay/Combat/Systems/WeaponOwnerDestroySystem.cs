using System;
using Game.Gameplay.Destroy;
using Scellecs.Morpeh;

namespace Game.Gameplay.Combat.Systems
{
	public sealed class WeaponOwnerDestroySystem : ICleanupSystem
	{
		private Filter _filter;

		private Stash<EquippedWeapon> _equippedStash;
		private Stash<DestroySelfRequest> _destroyStash;

		public World World { get; set; }

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<EquippedWeapon>()
				.With<DestroySelfRequest>()
				.Build();

			_equippedStash = World.GetStash<EquippedWeapon>();
			_destroyStash = World.GetStash<DestroySelfRequest>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var owner in _filter)
			{
				var weapon = _equippedStash.Get(owner).Value;

				if (!World.IsDisposed(weapon))
					_destroyStash.Set(weapon);
			}
		}

		void IDisposable.Dispose() { }
	}
}
