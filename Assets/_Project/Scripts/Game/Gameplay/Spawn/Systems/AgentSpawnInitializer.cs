using System;
using Game.Gameplay.Agent;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Spawn.Systems
{
	public sealed class AgentSpawnInitializer : IInitializer
	{
		private readonly AgentConfig _config;
		private readonly Vector3 _position;

		public World World { get; set; }

		public AgentSpawnInitializer(AgentConfig config, Vector3 position)
		{
			_config = config;
			_position = position;
		}

		void IInitializer.OnAwake()
		{
			World.CreateAgent(_config, _position);
		}

		void IDisposable.Dispose() { }
	}
}
