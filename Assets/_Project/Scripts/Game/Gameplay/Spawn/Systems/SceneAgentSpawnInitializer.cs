using System;
using System.Collections.Generic;
using Game.Gameplay.Agent;
using Scellecs.Morpeh;
using ViewComponent = Game.Gameplay.View.View;

namespace Game.Gameplay.Spawn.Systems
{
	public sealed class SceneAgentSpawnInitializer : IInitializer
	{
		private readonly IReadOnlyList<AgentAuthoring> _authorings;

		public World World { get; set; }

		public SceneAgentSpawnInitializer(IReadOnlyList<AgentAuthoring> authorings)
		{
			_authorings = authorings;
		}

		void IInitializer.OnAwake()
		{
			var viewStash = World.GetStash<ViewComponent>();

			foreach (var authoring in _authorings)
			{
				var entity = World.CreateAgent(authoring.Config, authoring.transform.position);
				var view = authoring.EntityInitializer;

				viewStash.Set(entity, new ViewComponent { Value = view });
				view.Bind(entity, World);
			}
		}

		void IDisposable.Dispose() { }
	}
}
