using System;
using Game.Gameplay.Input;
using MessagePipe;
using Scellecs.Morpeh;

namespace Game.Gameplay.Damage.Systems
{
	public sealed class PlayerDeathNotifySystem : ISystem
	{
		private readonly IPublisher<PlayerDiedMessage> _publisher;

		private Filter _filter;

		public World World { get; set; }

		public PlayerDeathNotifySystem(IPublisher<PlayerDiedMessage> publisher)
		{
			_publisher = publisher;
		}

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<PlayerControlled>()
				.With<Died>()
				.Build();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			if (_filter.IsEmpty())
				return;

			_publisher.Publish(new PlayerDiedMessage());
		}

		void IDisposable.Dispose() { }
	}
}
