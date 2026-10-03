using System;
using Scellecs.Morpeh;

namespace Game.Gameplay.Movement.Systems
{
	public class MovementAvailabilitySystem : IFixedSystem
	{
		private Filter _filter;
		private Stash<MovementBlockers> _blockersStash;
		private Stash<CanMove> _canMoveStash;
		
		public World World { get; set; }
		
		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<MovementBlockers>()
				.Build();

			_blockersStash = World.GetStash<MovementBlockers>();
			_canMoveStash = World.GetStash<CanMove>();
		}
		
		void ISystem.OnUpdate(float deltaTime)
		{
			foreach (var entity in _filter)
			{
				ref var blockers = ref _blockersStash.Get(entity);

				if (blockers.Value == MovementBlockReason.None)
				{
					_canMoveStash.Set(entity);
				}
				else
				{
					_canMoveStash.Remove(entity);
				}
			}
		}

		void IDisposable.Dispose() { }
	}
}