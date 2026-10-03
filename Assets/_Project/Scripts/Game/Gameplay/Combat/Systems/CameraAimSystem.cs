using System;
using Game.Gameplay.Common;
using Game.Gameplay.Input;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Combat.Systems
{
	public sealed class CameraAimSystem : ISystem
	{
		private Filter _filter;
		private Stash<AimRay> _aimStash;

		private readonly Camera _camera;

		public World World { get; set; }

		public CameraAimSystem(Camera camera)
		{
			_camera = camera;
		}

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<PlayerControlled>()
				.With<AimRay>()
				.Build();

			_aimStash = World.GetStash<AimRay>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			var cameraTransform = _camera.transform;

			foreach (var entity in _filter)
			{
				ref var aim = ref _aimStash.Get(entity);
				aim.Origin = cameraTransform.position;
				aim.Direction = cameraTransform.forward;
			}
		}

		void IDisposable.Dispose() { }
	}
}
