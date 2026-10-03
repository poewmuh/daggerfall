using System;
using Game.Gameplay.Input;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Movement.Systems
{
	public sealed class CameraFacingSystem : ISystem
	{
		private Filter _filter;
		private Stash<FacingDirection> _facingStash;

		private readonly Camera _camera;

		public World World { get; set; }

		public CameraFacingSystem(Camera camera)
		{
			_camera = camera;
		}

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<PlayerControlled>()
				.With<FacingDirection>()
				.Build();

			_facingStash = World.GetStash<FacingDirection>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			var forward = CameraPlanarBasis.From(_camera).Forward;

			foreach (var entity in _filter)
			{
				_facingStash.Get(entity).Value = forward;
			}
		}

		void IDisposable.Dispose() { }
	}
}
